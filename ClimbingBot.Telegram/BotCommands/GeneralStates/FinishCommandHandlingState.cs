using ClimbingBot.Telegram.BotCommands.Abstractions;

namespace ClimbingBot.Telegram.BotCommands.GeneralStates
{
    public class FinishCommandHandlingState : BaseState
    {
        public sealed override string Message { get; protected set; }
        
        public FinishCommandHandlingState(string message)
        {
            Message = message;
        }
    }
}