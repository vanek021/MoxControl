﻿using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Models.Enums;
using MoxControl.Infrastructure.Extensions;
using MoxControl.Infrastructure.Services;
using MoxControl.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MoxControl.Connect.Services.InternalServices
{
    public class BaseMachineService : BaseInternalService
    {
        public BaseMachineService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }

        protected async Task SendTelegramMachineNotify(string serverName, string machineName, MachineStatus oldStatus, MachineStatus newStatus)
        {
            if (oldStatus == newStatus)
                return;

            var notification = $"Изменился статус виртуальной машины! \n" +
                $"Название сервера: {serverName} \n" +
                $"Название виртуальной машины: {machineName} \n" +
                $"Старый статус: {oldStatus.GetDisplayName()} \n" +
                $"Новый статус: {newStatus.GetDisplayName()}";

            await SendTelegramAlert(notification);
        }
    }
}
