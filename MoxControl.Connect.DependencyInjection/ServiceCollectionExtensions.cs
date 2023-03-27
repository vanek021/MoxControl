using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Factory;
using MoxControl.Connect.Interfaces;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Services;

namespace MoxControl.Connect.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterConnectContexts(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<ConnectProxmoxDbContext>(options => options.UseNpgsql(connectionString));

            return serviceCollection;
        }

        public static IServiceCollection RegisterConnectServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IConnectServiceFactory, ConnectServiceFactory>();
            serviceCollection.AddScoped<IVirtualizationSystemClientFactory, VirtualizationSystemClientFactory>();
            serviceCollection.AddScoped<HangfireConnectManager>();

            return serviceCollection;
        }
    }
}