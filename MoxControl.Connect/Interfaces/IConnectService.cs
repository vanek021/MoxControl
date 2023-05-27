using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces.Connect;

namespace MoxControl.Connect.Interfaces
{
    public interface IConnectService
    {
        Task Initialize(IServiceScopeFactory serviceScopeFactory);
        public IServerService Servers { get; set; }
        public IMachineService Machines { get; set; }
    }
}
