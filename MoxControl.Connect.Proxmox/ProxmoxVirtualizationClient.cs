using MoxControl.Connect.Interfaces;
using Corsinvest.ProxmoxVE.Api;
using Microsoft.Extensions.DependencyInjection;

namespace MoxControl.Connect.Proxmox
{
    public class ProxmoxVirtualizationClient : IVirtualizationSystemClient
    {
        private readonly PveClient _pveClient;

        public ProxmoxVirtualizationClient(IServiceScopeFactory scopeFactory, string host, int port, string login, string password)
        {
            _pveClient = new PveClient(host, port);
            _pveClient.Login(login, password).GetAwaiter().GetResult();
        }

        public async Task GetServerRrdata()
        {
            
        }
    }
}