using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Enums;
using MoxControl.Connect.Interfaces;

namespace MoxControl.Connect.Factory
{
    public class VirtualizationSystemClientFactory
    {
        private readonly Dictionary<VirtualizationSystem, IVirtualizationSystemClient> _clients = new();

        public VirtualizationSystemClientFactory(IServiceScopeFactory serviceScopeFactory)
        {
            var baseType = typeof(IVirtualizationSystemClient);
        }

        public IVirtualizationSystemClient GetClientByVirtualizationSystem(VirtualizationSystem type)
        {
            if (!_clients.ContainsKey(type))
                throw new Exception("Tracker not exist");

            var result = _clients[type];
            return result is null ? throw new Exception() : result;
        }
    }
}