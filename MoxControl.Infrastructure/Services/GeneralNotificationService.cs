using MoxControl.Data;
using MoxControl.Models.Entities.Notifications;
using MoxControl.Models.Enums;
using Sakura.AspNetCore;

namespace MoxControl.Infrastructure.Services
{
    public class GeneralNotificationService
    {
        private readonly Database _db;
        public GeneralNotificationService(Database db)
        {
            _db = db;
        }

        private async Task<bool> AddNewNotifyAsync(GeneralNotifyData model)
        {
            _db.GeneralNotifications.Insert(new GeneralNotification()
            {
                Title = model.Title,
                Description = model.Description,
                Type = model.Type,
            });

            try
            {
                await _db.SaveChangesAsync();
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

        public async Task<List<GeneralNotification>> GetLastAsync(int count)
        {
            return await _db.GeneralNotifications.GetLast(count);
        }

        public async Task<IPagedList<GeneralNotification>> GetPagedAsync(int page, int pageSize)
        {
            return await _db.GeneralNotifications.GetPagedAsync(page, pageSize);
        }

        public async Task<GeneralNotification?> GetByIdAsync(long id)
        {
            return await _db.GeneralNotifications.GetByIdAsync(id);
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
