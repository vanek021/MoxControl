﻿using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using MoxControl.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Data
{
    [Injectable, Injectable(typeof(IDatabase))]
    public class Database : AbstractDatabase
    {
        public Database(DbContext context, GeneralSettingRepository generalSettingRepo, NotificationRepository notificationRepo,
            GeneralNotificationRepository generalNotificationRepo) : base(context)
        {
            GeneralSettings = generalSettingRepo;
            Notifications = notificationRepo;
            GeneralNotifications = generalNotificationRepo;
        }

        public GeneralSettingRepository GeneralSettings { get; private set; }

        public NotificationRepository Notifications { get; private set; }

        public GeneralNotificationRepository GeneralNotifications { get; private set; }
    }
}
