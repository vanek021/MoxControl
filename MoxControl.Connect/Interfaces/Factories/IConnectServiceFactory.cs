using MoxControl.Connect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
