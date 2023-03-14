using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Enums;
using MoxControl.Connect.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Infrastructure
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
