using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MoxControl.Connect.Proxmox.Controllers;
using MoxControl.Connect.Proxmox.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Proxmox
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterConnectProxmoxContext(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<ConnectProxmoxDbContext>(options => options.UseNpgsql(connectionString));

            return serviceCollection;
        }

        public static IServiceCollection RegisterConnectProxmoxServices(this IServiceCollection serviceCollection)
        {
            var assembly = typeof(ProxmoxSettingController).GetTypeInfo().Assembly;
            serviceCollection.AddControllersWithViews()
                .AddApplicationPart(assembly)
                .AddRazorRuntimeCompilation();

            serviceCollection.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            { options.FileProviders.Add(new EmbeddedFileProvider(assembly, "MoxControl.Connect.Proxmox")); });

            return serviceCollection;
        }
    }
}
