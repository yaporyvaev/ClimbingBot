using System;

namespace ClimbingBot.Telegram.Exceptions
{
    public class BotCommandException : ApplicationException
    {
        public BotCommandException()
        {
        }

        public BotCommandException(string message) : base(message)
        {
        }
        
        public BotCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}