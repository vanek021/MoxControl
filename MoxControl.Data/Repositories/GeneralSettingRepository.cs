﻿using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using MoxControl.Models.Entities.Settings;

namespace MoxControl.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<GeneralSetting>))]

    public class GeneralSettingRepository : WriteableRepository<GeneralSetting>
    {
        public GeneralSettingRepository(DbContext context) : base(context)
        {

        }

        protected override IQueryable<GeneralSetting> SingleWithIncludes()
        {
            return base.SingleWithIncludes()
                .Where(s => !s.IsHide);
        }

        protected override IQueryable<GeneralSetting> ManyWithIncludes()
        {
            return SingleWithIncludes();
        }

        public Task<List<GeneralSetting>> GetAll()
        {
            return ManyWithIncludes()
                .ToListAsync();
        }

        public string? GetValueBySystemName(string systemName)
        {
            return ManyWithIncludes()
                .SingleOrDefault(s => s.SystemName == systemName)?
                .Value;
        }
    }
}
