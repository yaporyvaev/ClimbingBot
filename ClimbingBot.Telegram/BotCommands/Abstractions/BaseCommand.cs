using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ClimbingBot.Telegram.BotCommands.Abstractions
{
    public abstract class BaseCommand
    {
        public abstract Task<CommandState> Handle(Message message, string payload);
    }
}