using System;
using ClimbingBot.Telegram.BotCommands.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ClimbingBot.Telegram.BotCommands
{
    public class CommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BaseCommand Create(string commandType)
        {
            return commandType switch
            {
                BotCommandsTypes.Hello => _serviceProvider.GetService<HelloCommand>(),
                BotCommandsTypes.Goodbye => _serviceProvider.GetService<GoodbyeCommand>(),
                BotCommandsTypes.Poll => _serviceProvider.GetService<CreatePollCommand>(),
                _ => null
            };
        }
    }
}