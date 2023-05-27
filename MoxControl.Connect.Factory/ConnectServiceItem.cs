using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Enums;

namespace MoxControl.Connect.Factory
{
    public class ConnectServiceItem : IConnectServiceItem
    {
        public ConnectServiceItem(VirtualizationSystem virtualizationSystem, IConnectService connectService)
        {
            VirtualizationSystem = virtualizationSystem;
            Service = connectService;
        }

        public VirtualizationSystem VirtualizationSystem { get; set; }
        public IConnectService Service { get; set; }
    }
}
