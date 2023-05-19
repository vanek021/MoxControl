using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Infrastructure.Extensions;
using MoxControl.Infrastructure.Services;
using MoxControl.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace MoxControl.Connect.Services.InternalServices
{
    public class BaseServerService : BaseInternalService
    {
        public BaseServerService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }

        protected async Task SendTelegramServerNotify(string serverName, ServerStatus oldStatus, ServerStatus newStatus)
        {
            if (oldStatus == newStatus)
                return;

            var notification = $"Изменился статус сервера! \n" +
                $"Название сервера: {serverName} \n" +
                $"Старый статус: {oldStatus.GetDisplayName()} \n" +
                $"Новый статус: {newStatus.GetDisplayName()}";

            await SendTelegramAlert(notification);
        }
    }
}
