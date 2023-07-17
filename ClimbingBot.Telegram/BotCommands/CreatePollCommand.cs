using System;
using System.Threading.Tasks;
using ClimbingBot.Services;
using ClimbingBot.Telegram.BotCommands.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace ClimbingBot.Telegram.BotCommands
{
    public class CreatePollCommand : BaseCommand
    {
        private readonly IServiceProvider _serviceProvider;

        public CreatePollCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<CommandState> Handle(Message message, string payload)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var pollService = serviceScope.ServiceProvider.GetService<PollService>();

            await pollService.CreateWorkoutParticipatingPoll(message.Chat.Id);
            
            return null;
        }
    }
}