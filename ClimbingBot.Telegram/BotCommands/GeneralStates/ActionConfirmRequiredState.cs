using ClimbingBot.Telegram.BotCommands.Abstractions;

namespace ClimbingBot.Telegram.BotCommands.GeneralStates
{
    public class ActionConfirmRequiredState : BaseState
    {
        public sealed override string Message { get; protected set; }

        public ActionConfirmRequiredState(string message)
        {
            Message = message;
        }
    }
}