using MoxControl.Connect.Interfaces;
using Corsinvest.ProxmoxVE.Api;
using Microsoft.Extensions.DependencyInjection;

namespace MoxControl.Connect.Proxmox
{
    public class ProxmoxVirtualizationClient : IVirtualizationSystemClient
    {
        private PveClient _pveClient;

        public async Task Initialize(IServiceScopeFactory scopeFactory, string host, int port, string login, string password)
        {
            _pveClient = new PveClient(host, port);
               
            await _pveClient.Login(login, password);
        }
    }
}