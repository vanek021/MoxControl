using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Services;
using MoxControl.Connect.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Factory
{
    public class ConnectServiceFactory : IConnectServiceFactory
    {
        private readonly Dictionary<VirtualizationSystem, IConnectService> _services = new();

        public ConnectServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            var proxmoxService = new ProxmoxConnectService();
            proxmoxService.Initialize(serviceScopeFactory).GetAwaiter().GetResult();
            _services.Add(VirtualizationSystem.Proxmox, proxmoxService);
        }

        public IConnectService GetByVirtualizationSystem(VirtualizationSystem virtualizationSystem)
        {
            if (!_services.ContainsKey(virtualizationSystem))
                throw new Exception("Service not exist");

            var result = _services[virtualizationSystem];

			return result is null ? throw new Exception() : result;
		}

        [Obsolete]
		public List<(VirtualizationSystem, IConnectService)> GetAllObsolete()
        {
            return _services
                .Select(x => (x.Key, x.Value))
                .ToList();
        }

        public List<IConnectServiceItem> GetAll()
        {
            return _services
                .Select(s => new ConnectServiceItem(s.Key, s.Value) as IConnectServiceItem)
                .ToList();
        }
    }
}
