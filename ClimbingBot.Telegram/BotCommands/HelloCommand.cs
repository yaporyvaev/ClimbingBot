using System;
using System.Linq;
using System.Threading.Tasks;
using ClimbingBot.Abstractions;
using ClimbingBot.Entities;
using ClimbingBot.Telegram.BotCommands.Abstractions;
using ClimbingBot.Telegram.BotCommands.GeneralStates;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace ClimbingBot.Telegram.BotCommands
{
    public class HelloCommand : BaseCommand
    {
        private readonly IServiceProvider _serviceProvider;

        public HelloCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<CommandState> Handle(Message message, string payload)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var repository = serviceScope.ServiceProvider.GetService<IRepository<TelegramGroup>>();
            
            var groupExists = repository.GetAll()
                .Any(s => s.GroupId == message.Chat.Id);

            if (groupExists)
            {
                return await Task.FromResult(new CommandState(BotCommandsTypes.Poll, message.From!.Id, 
                    new FinishCommandHandlingState("I've already known this group")));
            }

            await repository.Add(new TelegramGroup
            {
                GroupId = message.Chat.Id
            });
            
            return await Task.FromResult(new CommandState(BotCommandsTypes.Poll, message.From!.Id, 
                new FinishCommandHandlingState("Hello there!")));
        }
    }
}