using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Infrastructure.Extensions;

namespace MoxControl.Connect.Data.Seeds
{
    public static class ConnectSettingSeeds
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            var connectDbContext = scope.ServiceProvider.GetRequiredService<ConnectDbContext>();

            foreach (var virtualizationSystem in EnumExtensions.GetAllItems<VirtualizationSystem>())
            {
                var setting = connectDbContext.ConnectSettings.SingleOrDefault(x => x.VirtualizationSystem == virtualizationSystem);

                if (setting is null)
                {
                    connectDbContext.ConnectSettings.Add(new ConnectSetting { VirtualizationSystem = virtualizationSystem, IsSystemHasInterface = true });
                }
            }

            connectDbContext.SaveChanges();
        }
    }
}
