using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Data;
using MoxControl.Connect.Data.Seeds;
using MoxControl.Connect.Factory;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Proxmox;
using MoxControl.Connect.Services;
using MoxControl.Core.Extensions;

namespace MoxControl.Connect.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterConnectContexts(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<ConnectDbContext>(options => options.UseNpgsql(connectionString));

            serviceCollection.RegisterConnectProxmoxContext(connectionString);

            return serviceCollection;
        }

        public static IServiceCollection RegisterConnectServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterInjectableTypesFromAssemblies(typeof(ConnectDatabase));

            serviceCollection.AddScoped<IConnectServiceFactory, ConnectServiceFactory>();
            serviceCollection.AddScoped<HangfireConnectManager>();
            serviceCollection.AddScoped<ImageManager>();
            serviceCollection.AddScoped<TemplateManager>();

            serviceCollection.RegisterConnectProxmoxServices();

            return serviceCollection;
        }

        public static IApplicationBuilder UseConnect(this IApplicationBuilder app)
        {
            ConnectSettingSeeds.Seed(app.ApplicationServices);

            return app;
        }
    }
}