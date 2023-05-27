using MoxControl.Connect.Models.Enums;

namespace MoxControl.Connect.Interfaces.Factories
{
    public interface IConnectServiceFactory
    {
        public IConnectService GetByVirtualizationSystem(VirtualizationSystem virtualizationSystem);

        [Obsolete]
        public List<(VirtualizationSystem, IConnectService)> GetAllObsolete();

        public List<IConnectServiceItem> GetAll();
    }
}
