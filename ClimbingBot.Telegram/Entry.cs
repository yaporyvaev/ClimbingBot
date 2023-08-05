using System;
using System.Diagnostics.CodeAnalysis;
using ClimbingBot.Telegram.BotCommands;
using ClimbingBot.Telegram.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace ClimbingBot.Telegram
{
    public static class Entry
    {
        public static IServiceCollection AddTelegramIntegration<TOptions>([NotNull] this IServiceCollection serviceCollection, Action<TOptions> optionsAction)
            where TOptions : TelegramOptions, new()
        {
            var options = new TOptions();
            optionsAction?.Invoke(options);

            var settings = new TelegramOptions
            {
                TelegramBotApiKey = options.TelegramBotApiKey,
                TelegramLogChatId = options.TelegramLogChatId,
            };

            serviceCollection.AddSingleton(settings);
            serviceCollection.AddHostedService<ChannelMessageHandler>();
            serviceCollection.AddTransient(_ => new TelegramBotClient(settings.TelegramBotApiKey));
            
            serviceCollection.AddTransient<HelloCommand>();
            serviceCollection.AddTransient<GoodbyeCommand>();
            serviceCollection.AddTransient<CreatePollCommand>();

            serviceCollection.AddSingleton<CommandStateStore>();
            serviceCollection.AddTransient<CommandFactory>();
            serviceCollection.AddTransient<CommandHandler>();

            return serviceCollection;
        }
    }
}