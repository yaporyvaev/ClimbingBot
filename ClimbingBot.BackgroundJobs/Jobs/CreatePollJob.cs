using System.Threading.Tasks;
using ClimbingBot.Services;
using Quartz;

namespace ClimbingBot.BackgroundJobs.Jobs
{
    [DisallowConcurrentExecution]
    public class CreatePollJob : IJob
    {
        private readonly PollService _pollService;

        public CreatePollJob(PollService pollService)
        {
            _pollService = pollService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _pollService.CreateWorkoutParticipatingPoll(null);//TODO refactor argument
        }
    }
}