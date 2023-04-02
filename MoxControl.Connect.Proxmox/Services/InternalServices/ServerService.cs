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
        private readonly MoxControlUserManager _moxControlUserManager;
        private readonly ConnectProxmoxDbContext _context;

        public ServerService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<ConnectProxmoxDbContext>();
            _moxControlUserManager = scope.ServiceProvider.GetRequiredService<MoxControlUserManager>();
        }

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

        public async Task<BaseServer?> GetAsync(long id)
        {
            return await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<BaseServer>> GetAllAsync()
        {
            var servers = await _context.ProxmoxServers.Where(x => !x.IsDeleted).ToListAsync();
            return servers.Select(x => (BaseServer)x).ToList();
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

        public async Task<ServerHealthModel?> GetHealthModel(long serverId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == serverId && !x.IsDeleted);

            if (server is null)
                return null;

            var credentials = GetServerCredentials(server, initiatorUsername);

            var client = new ProxmoxVirtualizationClient(server.Host, server.Port, credentials.Login, credentials.Password);

            var rrddataItems = await client.GetServerRrdata();
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
