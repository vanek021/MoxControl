using MoxControl.Services;

namespace MoxControl.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<LdapService>();

            return serviceCollection;
        }
    }
}
