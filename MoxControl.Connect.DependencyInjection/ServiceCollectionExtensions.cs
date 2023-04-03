using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MoxControl.Connect.Data;
using MoxControl.Connect.Data.Seeds;
using MoxControl.Connect.Factory;
using MoxControl.Connect.Interfaces.Factories;
using MoxControl.Connect.Proxmox.Controllers;
using MoxControl.Connect.Proxmox.Data;
using MoxControl.Connect.Proxmox.Services;
using MoxControl.Connect.Services;
using MoxControl.Core.Extensions;
using System.Reflection;

namespace MoxControl.Connect.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterConnectContexts(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<ConnectDbContext>(options => options.UseNpgsql(connectionString));
            serviceCollection.AddDbContext<ConnectProxmoxDbContext>(options => options.UseNpgsql(connectionString));

            return serviceCollection;
        }

        public static IServiceCollection RegisterConnectServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterInjectableTypesFromAssemblies(typeof(ConnectDatabase));

            serviceCollection.AddScoped<IConnectServiceFactory, ConnectServiceFactory>();
            serviceCollection.AddScoped<IVirtualizationSystemClientFactory, VirtualizationSystemClientFactory>();
            serviceCollection.AddScoped<HangfireConnectManager>();
            serviceCollection.AddScoped<ImageManager>();
            serviceCollection.AddScoped<TemplateManager>();

            var assembly = typeof(ProxmoxSettingController).GetTypeInfo().Assembly;
            serviceCollection.AddMvc(x => x.EnableEndpointRouting = false)
                .AddApplicationPart(assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return serviceCollection;
        }

        public static IApplicationBuilder UseConnect(this IApplicationBuilder app)
        {
            ConnectSettingSeeds.Seed(app.ApplicationServices);

            return app;
        }
    }
}