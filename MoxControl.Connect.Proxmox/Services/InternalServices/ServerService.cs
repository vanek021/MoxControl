using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces.Connect;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Models.Entities;
using MoxControl.Connect.Proxmox.VirtualizationClient.Helpers;
using MoxControl.Connect.Services.InternalServices;
using MoxControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox.Services.InternalServices
{
    public class ServerService : BaseServerService, IServerService
    {
        private readonly ConnectProxmoxDbContext _context;

        public ServerService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<ConnectProxmoxDbContext>();
        }

        #region lambdas

        public async Task<BaseServer?> GetAsync(long id)
            => await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        public async Task<List<BaseServer>> GetAllAsync()
            => await _context.ProxmoxServers.Where(x => !x.IsDeleted).Select(x => (BaseServer)x).ToListAsync();

        public async Task<int> GetTotalCountAsync()
            => await _context.ProxmoxServers.Where(x => !x.IsDeleted).CountAsync();

        public async Task<int> GetAliveCountAsync()
            => await _context.ProxmoxServers.Where(x => x.Status == ServerStatus.Running).CountAsync();

        #endregion

        public async Task<bool> CreateAsync(string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null, string? initiatorUsername = null)
        {
            var server = new ProxmoxServer()
            {
                Host = host,
                Port = port,
                AuthorizationType = authorizationType,
                Name = name,
                Description = description,
                RootLogin = rootLogin,
                RootPassword = rootPassword
            };

            _context.ProxmoxServers.Add(server);

            try
            {
                await _context.SaveChangesAsync();
                BackgroundJob.Enqueue<HangfireConnectManager>(x => x.HangfireSendServerHeartBeat(VirtualizationSystem.Proxmox, server.Id, initiatorUsername));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(long id, string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (server is null)
                return false;

            server.Host = host;
            server.Port = port;
            server.AuthorizationType = authorizationType;
            server.Name = name;
            server.Description = description;
            server.RootLogin = rootLogin;
            server.RootPassword = rootPassword;

            _context.ProxmoxServers.Update(server);

            try
            {
                await _context.SaveChangesAsync();
                BackgroundJob.Enqueue<HangfireConnectManager>(x => x.HangfireSendServerHeartBeat(VirtualizationSystem.Proxmox, server.Id, initiatorUsername));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (server is null)
                return false;

            server.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task SendHeartBeat(long serverId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == serverId && !x.IsDeleted);

            if (server is null)
                return;

            var credentials = GetServerCredentials(server, initiatorUsername);

            try
            {
                var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(server.Host, server.Port, credentials.Login, credentials.Password);

                server.Status = ServerStatus.Running;
            }
            catch
            {
                server.Status = ServerStatus.Unknown;
            }
            finally
            {
                _context.Update(server);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendHeartBeatToAll()
        {
            var serverIds = await _context.ProxmoxServers.Where(x => !x.IsDeleted).Select(x => x.Id).ToListAsync();

            foreach (var serverId in serverIds)
            {
                await SendHeartBeat(serverId);
            }
        }

        public async Task SyncMachines(long serverId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers
                .Include(s => s.Machines)
                .FirstOrDefaultAsync(x => x.Id == serverId && !x.IsDeleted);

            if (server is null)
                return;

            var credentials = GetServerCredentials(server, initiatorUsername);

            var proxmoxVirtualizationSystem = new ProxmoxVirtualizationClient(server.Host, server.Port, credentials.Login, credentials.Password);

            var machines = await proxmoxVirtualizationSystem.GetNodeMachines();

            foreach (var machine in machines)
            {
                var dbMachine = server.Machines.FirstOrDefault(m => m.ProxmoxId.HasValue && m.ProxmoxId.Value == machine.VMid);

                if (dbMachine is not null)
                {
                    dbMachine.Name = machine.Name ?? dbMachine.Name;
                    dbMachine.ProxmoxName = machine.Name;
                    dbMachine.Status = machine.Status.GetMachineStatus();
                    _context.ProxmoxMachines.Update(dbMachine);
                    
                }
                else
                {
                    var newMachine = new ProxmoxMachine()
                    {
                        Name = machine.Name ?? $"Proxmox Machine {machine.VMid}",
                        Description = $"Proxmox Machine {machine.VMid}",
                        Status = machine.Status.GetMachineStatus(),
                        ProxmoxName = machine.Name,
                        ProxmoxId = machine.VMid,
                        Stage = MachineStage.Using,
                        CPUCores = machine.CPUs,
                        RAMSize = Convert.ToInt32(machine.MemoryTotal / (1024 * 1024)),
                        HDDSize = Convert.ToInt32(machine.HDDTotal / (1024 * 1024)),
                        ServerId = serverId
                    };
                    _context.ProxmoxMachines.Add(newMachine);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ServerHealthModel?> GetHealthModel(long serverId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == serverId && !x.IsDeleted);

            if (server is null)
                return null;

            var credentials = GetServerCredentials(server, initiatorUsername);

            var client = new ProxmoxVirtualizationClient(server.Host, server.Port, credentials.Login, credentials.Password);

            var rrddataItems = await client.GetServerRrddata();
            var lastData = rrddataItems.LastOrDefault();

            if (lastData is null)
                return null;

            var serverHealthModel = new ServerHealthModel(long.Parse(lastData.MemoryTotal!),
                double.Parse(lastData.MemoryUsed!, CultureInfo.InvariantCulture), long.Parse(lastData.HDDTotal!),
                double.Parse(lastData.HDDUsed!, CultureInfo.InvariantCulture), double.Parse(lastData.CPUUsed!, CultureInfo.InvariantCulture));

            return serverHealthModel;
        }
    }
}
