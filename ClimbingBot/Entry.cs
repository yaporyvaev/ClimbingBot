using ClimbingBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClimbingBot
{
    public static class Entry
    {
        public static IServiceCollection AddBot(this IServiceCollection serviceCollection) 
        {
            serviceCollection.AddTransient<PollService>();

            return serviceCollection;
        }
    }
}