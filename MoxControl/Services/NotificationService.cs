using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Extensions;
using MoxControl.Data;
using MoxControl.Infrastructure.Services;
using MoxControl.Models.Entities;
using MoxControl.Models.Entities.Notifications;
using MoxControl.Models.Enums;

namespace MoxControl.Services
{
    public class NotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MoxControlUserManager _moxControlUserManager;
        private readonly Database _db;

        public NotificationService(Database db, MoxControlUserManager moxControlUserManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _moxControlUserManager = moxControlUserManager;
            _db = db;
        }

        public async Task<List<Notification>> GetActiveUserNotificationsAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context is null)
                return new();

            var userId = context.User.GetUserId();

            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var id))
                return new();

            return await _db.Notifications.GetActiveForUser(id);
        }

        public async Task MarkAsViewedAsync(long notificationId)
        {
            var notification = _db.Notifications.GetById(notificationId);
            if (notification != null)
            {
                notification.WasViewed = true;
                await _db.SaveChangesAsync();
            }
        }

        public async Task MarkAsViewedRangeAsync(List<long> notificationIds)
        {
            foreach (var id in notificationIds)
                await MarkAsViewedAsync(id);
        }

        public async Task<bool> AddErrorAsync(string userName, string title, string description)
        {
            var model = new NotifyData(NotificationType.Error, title, description);
            return await AddNewNotifyAsync(userName, model);
        }

        private async Task<bool> AddNewNotifyAsync(string userName, NotifyData model)
        {
            var user = GetUser(userName);
            if (user == null)
                return false;

            user.NotificationReceiver.Notifications.Add(new Notification()
            {
                Title = model.Title,
                Description = model.Description,
                NotificationType = model.Type,
                WasViewed = false
            });

            await _moxControlUserManager.UpdateAsync(user);
            //await _hubContext.Clients.User(user.Id.ToString()).SendAsync("OnNotificationsUpdate");
            return true;
        }

        private User? GetUser(string userName)
        {
            return _moxControlUserManager.Users
                .Include(x => x.NotificationReceiver)
                    .ThenInclude(x => x.Notifications)
                .FirstOrDefault(x => x.UserName == userName);
        }
    }

    public class NotifyData
    {
        public NotifyData(NotificationType type, string title, string description)
        {
            Title = title;
            Description = description;
            Type = type;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType Type { get; set; }
    }
}
