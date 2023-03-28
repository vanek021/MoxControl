using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Models.Entities;
using MoxControl.Infrastructure.Services;
using Sakura.AspNetCore;

namespace MoxControl.Connect.Proxmox.Services
{
    public class ProxmoxService : IConnectService
    {
        private MoxControlUserManager _moxControlUserManager;     
        private ConnectProxmoxDbContext _context;
        private IVirtualizationSystemClientFactory _virtualizationSystemClientFactory;

        public Task Initialize(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<ConnectProxmoxDbContext>();          
            _moxControlUserManager = scope.ServiceProvider.GetRequiredService<MoxControlUserManager>();
            _virtualizationSystemClientFactory = scope.ServiceProvider.GetRequiredService<IVirtualizationSystemClientFactory>();

            return Task.CompletedTask;
        }

        public async Task<bool> CreateServerAsync(string host, int port, AuthorizationType authorizationType, string name,
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

        public async Task<bool> UpdateServerAsync(long id, string host, int port, AuthorizationType authorizationType, string name, 
            string description, string? rootLogin = null, string? rootPassword = null, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id);

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

        public async Task<BaseServer?> GetServerAsync(long id)
        {
            return await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<BaseServer>> GetAllServersAsync()
        {
            var servers = await _context.ProxmoxServers.ToListAsync();
            return servers.Select(x => (BaseServer)x).ToList();
        }

        public async Task HangfireSendServerHeartBeat(long serverId, string? initiatorUsername = null)
        {
            var server = await _context.ProxmoxServers.FirstOrDefaultAsync(x => x.Id == serverId);

            if (server is null)
                return;

            string? userName = null; 
            string? password = null;

            if (!string.IsNullOrEmpty(initiatorUsername))
            {
                userName = initiatorUsername;
                password = _moxControlUserManager.GetUserPasswordFromProtectedFile(initiatorUsername);
            }
            else
            {
                userName = server.RootLogin;
                password = server.RootPassword;
            }

            if (userName is null || password is null)
                throw new ArgumentException("Не удалось найти данные авторизации сервера");

            try
            {
                var proxmoxVirtualizationSystem = await _virtualizationSystemClientFactory.GetClientByVirtualizationSystemAsync(VirtualizationSystem.Proxmox,
                server.Host, server.Port, userName, password);

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
    }
}
