using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ClimbingBot.Telegram
{
    public static class TelegramNotification
    {
        public static async Task SendNotification(IServiceProvider serviceProvider, string text)
        {
            using var scope = serviceProvider.CreateScope();
            var options = scope.ServiceProvider.GetService<TelegramOptions>();

            var tgClient = scope.ServiceProvider.GetService<TelegramBotClient>();
            await tgClient.SendTextMessageAsync(new ChatId(options.TelegramLogChatId), text);
        }
    }
}
