using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using MoxControl.Models.Entities;
using MoxControl.Models.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<Notification>))]
    public class NotificationRepository : WriteableRepository<Notification>
    {
        public NotificationRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<Notification> SingleWithIncludes()
        {
            return base.SingleWithIncludes();
        }

        protected override IQueryable<Notification> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<List<Notification>> GetActiveForUser(long userId)
        {
            var userNotifications = ManyWithIncludes()
                .Include(x => x.NotificationReceiver)
                .ThenInclude(x => x.User)
                .Where(x => x.NotificationReceiver.UserId == userId);
            return userNotifications
                .Where(x => !x.WasViewed)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public Notification? GetNotificationById(long id)
        {
            return ManyWithIncludes()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }
    }
}
