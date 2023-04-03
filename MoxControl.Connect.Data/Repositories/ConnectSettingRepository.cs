﻿using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<ConnectSetting>))]
    public class ConnectSettingRepository : WriteableRepository<ConnectSetting>
    {
        public ConnectSettingRepository(ConnectDbContext context) : base(context)
        {

        }

        public Task<ConnectSetting> GetByVirtualizationSystemAsync(VirtualizationSystem virtualizationSystem)
        {
            return ManyWithIncludes().SingleAsync(x => x.VirtualizationSystem == virtualizationSystem);
        }
    }
}
