using System;
using ClimbingBot.BackgroundJobs.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ClimbingBot.BackgroundJobs
{
    public static class Entry
    {
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                
                var endGameCheckerJobKey = new JobKey("sendPollJob");
                q.AddJob<CreatePollJob>(opts => opts.WithIdentity(endGameCheckerJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(endGameCheckerJobKey)
                    .WithIdentity("sendPollJob-trigger")
                    //.WithCronSchedule("1 * * ? * *"));
                    .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(9, 00, DayOfWeek.Monday,
                        DayOfWeek.Wednesday, DayOfWeek.Friday)));
                //.WithCronSchedule("0 0 9 ? * MON,WED,FRI *"));
            });
            
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}