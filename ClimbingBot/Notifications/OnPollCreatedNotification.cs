using ClimbingBot.Models;
using MediatR;

namespace ClimbingBot.Notifications
{
    public class OnPollCreatedNotification : INotification
    {
        public Poll Poll { get; }
        public long? ChatId { get; }
        
        public OnPollCreatedNotification(Poll poll, long? chatId)
        {
            Poll = poll;
            ChatId = chatId;
        }
        
    }
}