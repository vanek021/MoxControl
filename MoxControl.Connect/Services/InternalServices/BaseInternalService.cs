using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Models.Enums;
using MoxControl.Data;
using MoxControl.Infrastructure.Services;
using MoxControl.Models.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MoxControl.Connect.Services.InternalServices
{
    public abstract class BaseInternalService
    {
        protected readonly IConfiguration _configuration;
        protected readonly MoxControlUserManager _moxControlUserManager;
        protected readonly TelegramService _telegramService;
        protected readonly GeneralNotificationService _generalNotificationService;
        protected readonly ImageManager _imageManager;
        protected readonly TemplateManager _templateManager;
        protected readonly Database _db;

        public BaseInternalService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            _moxControlUserManager = scope.ServiceProvider.GetRequiredService<MoxControlUserManager>();
            _telegramService = scope.ServiceProvider.GetRequiredService<TelegramService>();
            _generalNotificationService = scope.ServiceProvider.GetRequiredService<GeneralNotificationService>();
            _imageManager = scope.ServiceProvider.GetRequiredService<ImageManager>();
            _templateManager = scope.ServiceProvider.GetRequiredService<TemplateManager>();
            _db = scope.ServiceProvider.GetRequiredService<Database>();
        }

        protected virtual ServerCredentialsModel GetServerCredentials(BaseServer server, string? initiatorUsername = null)
        {
            string? userName = null;
            string? password = null;

            if (server.AuthorizationType == AuthorizationType.UserCredentials && !string.IsNullOrEmpty(initiatorUsername))
            {
                userName = initiatorUsername;
                password = _moxControlUserManager.GetUserPasswordFromProtectedFile(initiatorUsername);
            }

            if (server.AuthorizationType == AuthorizationType.RootCredentials)
            {
                userName = server.RootLogin;
                password = server.RootPassword;
            }

            if (userName is null || password is null)
                throw new ArgumentException("Не удалось найти данные авторизации сервера");

            return new ServerCredentialsModel()
            {
                Login = userName,
                Password = password
            };
        }

        protected async Task SendTelegramAlert(string content)
        {
            var telegramChatId = _db.GeneralSettings.GetValueBySystemName(SettingConstants.TelegramChat);

            if (string.IsNullOrEmpty(telegramChatId))
                return;

            try
            {
                await _telegramService.TelegramBotClient.SendTextMessageAsync(new ChatId(telegramChatId), content);
            }
            catch (Exception ex)
            {
                await _generalNotificationService.AddInternalServerErrorAsync(ex);
            }
        }
    }
}
