using MoxControl.Connect.Models.Enums;

namespace MoxControl.Connect.Interfaces.Factories
{
    public interface IConnectServiceItem
    {
        public VirtualizationSystem VirtualizationSystem { get; set; }
        public IConnectService Service { get; set; }
    }
}
