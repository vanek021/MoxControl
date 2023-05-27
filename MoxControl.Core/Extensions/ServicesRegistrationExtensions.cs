using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Core.Attributes;
using MoxControl.Core.Data;
using MoxControl.Core.Internal;
using MoxControl.Core.Models;
using System.Reflection;

namespace MoxControl.Core.Extensions
{
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection AddLowercaseRouting(this IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = options.AccessDeniedPath.ToString().ToLowerInvariant();
                options.LoginPath = options.LoginPath.ToString().ToLowerInvariant();
                options.LogoutPath = options.LogoutPath.ToString().ToLowerInvariant();
                //options.ReturnUrlParameter = options.ReturnUrlParameter.ToString().ToLowerInvariant();
            });

            return services;
        }

        public static IServiceCollection AddBasePgsqlContext<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext
        {
            services.AddBaseDbContext<TContext>((options => options.UseNpgsql(connectionString)));
            return services;
        }

        public static IServiceCollection AddBaseDbContext<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(optionsAction);
            services.AddScoped<DbContext>(x => x.GetService<TContext>());

            ReflectionTools.CallGenericStaticMethodForDbContextType<TContext>(typeof(ServicesRegistrationExtensions), nameof(RegisterGenericBaseAbilityContext), new object[] { services });

            return services;
        }

        public static IServiceCollection RegisterInjectableTypesFromAssemblies(this IServiceCollection services, params Type[] assemblyMarkerTypes)
        {
            var assemblies = assemblyMarkerTypes.Select(x => x.Assembly);
            foreach (var asm in assemblies)
            {
                var all = asm.GetTypes().Where(x => x.CustomAttributes.Any(it => it.AttributeType.FullName == "MoxControl.Core.Attributes.InjectableAttribute"));
                foreach (var it in all)
                    RegisterInjectableType(services, it);
            }

            return services;
        }

        private static void RegisterInjectableType(IServiceCollection services, Type type)
        {
            services.AddScoped(type);

            var attrs = type.GetCustomAttributes(typeof(InjectableAttribute), true).OfType<InjectableAttribute>();
            foreach (var atr in attrs)
            {
                if (atr.ServiceType != null)
                    services.AddScoped(atr.ServiceType, x => x.GetRequiredService(type));
            }
        }

        [Obfuscation(Exclude = true)]
        private static void RegisterGenericBaseAbilityContext<TContext, TUser, TRole>(IServiceCollection services)
            where TContext : BaseDbContext<TUser, TRole>
            where TUser : BaseUser
            where TRole : BaseRole
        {
            services.AddScoped<BaseDbContext<TUser, TRole>>(x => x.GetService<TContext>());
        }
    }
}
