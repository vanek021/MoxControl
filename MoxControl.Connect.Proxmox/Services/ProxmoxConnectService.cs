using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Interfaces.Connect;
using MoxControl.Connect.Proxmox.Services.InternalServices;

namespace MoxControl.Connect.Proxmox.Services
{
#pragma warning disable CS8618

    public class ProxmoxConnectService : IConnectService
    {
        public IServerService Servers { get; set; }
        public IMachineService Machines { get; set; }

        public Task Initialize(IServiceScopeFactory serviceScopeFactory)
        {
            Servers = new ServerService(serviceScopeFactory);
            Machines = new MachineService(serviceScopeFactory);
            return Task.CompletedTask;
        }
    }
#pragma warning restore CS8618
}
