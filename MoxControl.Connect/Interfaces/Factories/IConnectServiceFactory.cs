using MoxControl.Connect.Models.Enums;

namespace MoxControl.Connect.Interfaces.Factories
{
    public interface IConnectServiceFactory
    {
        public IConnectService GetByVirtualizationSystem(VirtualizationSystem virtualizationSystem);

        public List<IConnectServiceItem> GetAll();
    }
}
