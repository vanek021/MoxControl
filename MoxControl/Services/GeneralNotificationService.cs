﻿using Microsoft.EntityFrameworkCore;
using MoxControl.Data;
using MoxControl.Infrastructure.Services;
using MoxControl.Models.Entities.Notifications;
using MoxControl.Models.Enums;
using MoxControl.Services.Abtractions;
using Sakura.AspNetCore;

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

        public async Task<List<GeneralNotification>> GetLastAsync(int count)
        {
            return await DbContext.GeneralNotifications.OrderBy(n => n.CreatedAt).Take(count).ToListAsync();
        }

        public async Task<IPagedList<GeneralNotification>> GetPagedAsync(int page, int pageSize)
        {
            return await DbContext.GeneralNotifications.OrderBy(n => n.CreatedAt).ToPagedListAsync(pageSize, page);
        }

        public async Task<GeneralNotification?> GetById(long id)
        {
            return await DbContext.GeneralNotifications.FirstOrDefaultAsync(n => n.Id == id);
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
