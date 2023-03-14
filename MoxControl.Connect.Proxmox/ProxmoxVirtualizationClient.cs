using MoxControl.Connect.Interfaces;
using Corsinvest.ProxmoxVE.Api;
using Microsoft.Extensions.DependencyInjection;

namespace MoxControl.Connect.Proxmox
{
    public class ProxmoxVirtualizationClient : IVirtualizationSystemClient
    {
        public ProxmoxVirtualizationClient()
        {

        }

        public Task Initialize(IServiceScopeFactory scopeFactory)
        {
            throw new NotImplementedException();
        }
    }
}