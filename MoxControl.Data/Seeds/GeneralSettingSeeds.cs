using Microsoft.Extensions.DependencyInjection;
using MoxControl.Models.Constants;
using MoxControl.Models.Entities.Settings;

namespace MoxControl.Data.Seeds
{
    public static class GeneralSettingSeeds
    {
        private static List<GeneralSetting> generalSettings = new()
        {
            new GeneralSetting()
            {
                SystemName = SettingConstants.TelegramChat,
                Description = "Чат телеграм для отправки уведомлений",
                IsHide = false,
                Value = "5865718013"
            }
        };

        public static void Seed(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            foreach (var setting in generalSettings)
            {
                var dbSetting = ctx.GeneralSettings.FirstOrDefault(s => s.SystemName == setting.SystemName);

                if (dbSetting is null)
                {
                    ctx.GeneralSettings.Add(setting);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
