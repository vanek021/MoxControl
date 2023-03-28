using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox.Services;
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
            var proxmoxService = new ProxmoxService();

            proxmoxService.Initialize(serviceScopeFactory);

            _services.Add(VirtualizationSystem.Proxmox, proxmoxService);
        }

        public IConnectService GetByVirtualizationSystem(VirtualizationSystem virtualizationSystem)
        {
            if (!_services.ContainsKey(virtualizationSystem))
                throw new Exception("Service not exist");

            var result = _services[virtualizationSystem];

            if (result is null)
                throw new Exception();

            return result;
        }

        public List<(VirtualizationSystem, IConnectService)> GetAll()
        {
            return _services.Select(x => (x.Key, x.Value)).ToList();
        }
    }
}
