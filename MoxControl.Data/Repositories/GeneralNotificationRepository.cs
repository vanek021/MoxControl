﻿using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using MoxControl.Models.Entities.Notifications;
using Sakura.AspNetCore;

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
                .OrderByDescending(n => n.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IPagedList<GeneralNotification>> GetPagedAsync(int page, int pageSize)
        {
            return await ManyWithIncludes()
                .OrderByDescending(n => n.CreatedAt)
                .ToPagedListAsync(pageSize, page);
        }
    }
}
