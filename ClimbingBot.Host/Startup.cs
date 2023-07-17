using System;
using System.Linq;
using System.Reflection;
using ClimbingBot.BackgroundJobs;
using ClimbingBot.Database;
using ClimbingBot.Telegram;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClimbingBot
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnString = Configuration["DbConnectionString"];
            services.AddPostgreSqlStorage(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseNpgsql(dbConnString);
            });
            
            services.AddMemoryCache();
            services.AddHealthChecks();
            
            services.AddBot();

            services.AddBackgroundJobs();
            
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddTelegramIntegration<TelegramOptions>(options =>
            {
                options.TelegramBotApiKey = Configuration["Telegram:ApiKey"];
                options.TelegramLogChatId = long.Parse(Configuration["Telegram:LogChatId"]);
            });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IServiceProvider serviceProvider)
        {
            MigrationsRunner.ApplyMigrations(logger, serviceProvider, "ClimbingBot.Host").Wait();

            app.UseHealthChecks("/health");
        }
    }
}
