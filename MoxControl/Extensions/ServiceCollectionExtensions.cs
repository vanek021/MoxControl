using MoxControl.Data.Seeds;
using MoxControl.Infrastructure.Configurations;
using MoxControl.Infrastructure.Services;
using MoxControl.Services;

namespace MoxControl.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<UserService>();
            serviceCollection.AddScoped<LdapService>();
            serviceCollection.AddScoped<ServerService>();
            serviceCollection.AddScoped<MachineService>();
            serviceCollection.AddScoped<GeneralNotificationService>();
            serviceCollection.AddScoped<MoxControlUserManager>();

            return serviceCollection;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<ADConfig>(configuration.GetSection("AD"));

            return serviceCollection;
        }

        public static void SeedData(this IServiceProvider serviceProvider)
        {
            RoleSeeds.Seed(serviceProvider);
        }
    }
}
