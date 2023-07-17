using System.Threading.Tasks;
using ClimbingBot.Models;
using ClimbingBot.Notifications;
using MediatR;

namespace ClimbingBot.Services
{
    public class PollService
    {
        private readonly IMediator _mediator;

        public PollService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CreateWorkoutParticipatingPoll(long? chatId)
        {
            var poll = new Poll
            {
                Title = "Занятие сегодня",
                Options = new[]
                {
                    "Иду",
                    "Иду +1",
                    "Иду +2",
                    "Иду болдер",
                    "Нет"
                }
            };
            
            await _mediator.Publish(new OnPollCreatedNotification(poll, chatId));
        } 
    }
}