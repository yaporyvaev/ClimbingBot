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
    public class GoodbyeCommand : BaseCommand
    {
        private readonly IServiceProvider _serviceProvider;

        public GoodbyeCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<CommandState> Handle(Message message, string payload)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var repository = serviceScope.ServiceProvider.GetService<IRepository<TelegramGroup>>();
            
            var group = repository.GetAll()
                .FirstOrDefault(s => s.GroupId == message.Chat.Id);

            if (group == null)
            {
                return await Task.FromResult(new CommandState(BotCommandsTypes.Goodbye, message.From!.Id,
                    new FinishCommandHandlingState("I don't known this group yet")));
            }

            await repository.Remove(group);
                
            return await Task.FromResult(new CommandState(BotCommandsTypes.Goodbye, message.From!.Id, 
                new FinishCommandHandlingState("Goodbye :(")));
        }
    }
}