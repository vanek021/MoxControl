using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Models.Enums;
using MoxControl.Connect.Proxmox;
using System.Reflection;

namespace MoxControl.Connect.Factory
{
    public class VirtualizationSystemClientFactory : IVirtualizationSystemClientFactory
    {
        private readonly Dictionary<VirtualizationSystem, IVirtualizationSystemClient> _clients = new();

        // Сюда добавлять сборки, реализующие клиент взаимодействия с системами виртуализации
        private readonly List<Assembly> _assemblies = new() { typeof(ProxmoxVirtualizationClient).Assembly };

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