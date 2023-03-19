using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Models.Entities;

namespace MoxControl.Connect.Proxmox.Services
{
    public class ProxmoxService : IConnectService
    {
        private ConnectProxmoxDbContext _context;

        public Task Initialize(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<ConnectProxmoxDbContext>();

            return Task.CompletedTask;
        }

        public async Task<bool> CreateServerAsync(string host, int port, AuthorizationType authorizationType, string name,
            string description, string? rootLogin = null, string? rootPassword = null)
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateServerAsync(long id, string host, int port, AuthorizationType authorizationType, string name, 
            string description, string? rootLogin = null, string? rootPassword = null)
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
    }
}
