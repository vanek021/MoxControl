﻿using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Services.InternalServices
{
    public class BaseServerService : BaseInternalService
    {
        public BaseServerService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }
    }
}
