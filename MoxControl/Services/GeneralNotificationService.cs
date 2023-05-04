using MoxControl.Data;
using MoxControl.Infrastructure.Services;
using MoxControl.Models.Entities.Notifications;
using MoxControl.Models.Enums;
using MoxControl.Services.Abtractions;

namespace MoxControl.Services
{
    public class GeneralNotificationService : ServiceBase<GeneralNotification>
    {
        public GeneralNotificationService(AppDbContext dbContext) : base(dbContext)
        {

        }

        private async Task<bool> AddNewNotifyAsync(GeneralNotifyData model)
        {
            DbContext.GeneralNotifications.Add(new GeneralNotification()
            {
                Title = model.Title,
                Description = model.Description,
                Type = model.Type,
            });

            try
            {
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddInternalServerErrorAsync(Exception exception)
        {
            var model = new GeneralNotifyData(GeneralNotificationType.InternalServerError, "Внутренняя ошибка сервера", exception.ToString());
            return await AddNewNotifyAsync(model);
        }

    }

    public class GeneralNotifyData
    {
        public GeneralNotifyData(GeneralNotificationType type, string title, string description)
        {
            Title = title;
            Description = description;
            Type = type;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public GeneralNotificationType Type { get; set; }
    }
}
