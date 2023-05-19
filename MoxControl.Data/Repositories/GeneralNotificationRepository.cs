using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using MoxControl.Models.Entities.Notifications;
using MoxControl.Models.Entities.Settings;
using Sakura.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<GeneralNotification>))]
    public class GeneralNotificationRepository : WriteableRepository<GeneralNotification>
    {
        public GeneralNotificationRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<GeneralNotification> SingleWithIncludes()
        {
            return base.SingleWithIncludes();
        }

        protected override IQueryable<GeneralNotification> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<List<GeneralNotification>> GetLast(int count)
        {
            return ManyWithIncludes()
                .OrderBy(n => n.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IPagedList<GeneralNotification>> GetPagedAsync(int page, int pageSize)
        {
            return await ManyWithIncludes()
                .OrderBy(n => n.CreatedAt)
                .ToPagedListAsync(pageSize, page);
        }
    }
}
