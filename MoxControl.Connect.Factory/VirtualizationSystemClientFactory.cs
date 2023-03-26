using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox;
using System.Reflection;

namespace MoxControl.Connect.Factory
{
    public class VirtualizationSystemClientFactory : IVirtualizationSystemClientFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public VirtualizationSystemClientFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;   
        }

        public async Task<IVirtualizationSystemClient> GetClientByVirtualizationSystemAsync(VirtualizationSystem type, string host, int port, string login, string password)
        {
            switch (type)
            {
                case VirtualizationSystem.Proxmox:
                    var client = new ProxmoxVirtualizationClient();
                    await client.Initialize(_serviceScopeFactory, host, port, login, password);
                    return client;
                default:
                    throw new NotImplementedException("Unknown Virtualization System");
            }
        }
    }
}