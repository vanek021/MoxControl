using Microsoft.Extensions.Options;
using MoxControl.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MoxControl.Infrastructure.Services
{
    public class TelegramService
    {
        public TelegramBotClient TelegramBotClient { get; set; }

        private readonly TelegramConfig _telegramConfig;

        public TelegramService(IOptions<TelegramConfig> options)
        {
            _telegramConfig = options.Value;
            TelegramBotClient = new TelegramBotClient(_telegramConfig.Token);
        }
    }
}
