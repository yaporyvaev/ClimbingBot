using System;
using ClimbingBot.Telegram;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.PeriodicBatching;
using Telegram.Bot;

namespace ClimbingBot.Logging
{
    public static class TeleSinkExtensions
    {
        public static LoggerConfiguration TeleSink(
            this LoggerSinkConfiguration config,
            IServiceProvider services,
            LogEventLevel minimumLevel = LogEventLevel.Error)
        {
            var tgClient = (TelegramBotClient)services.GetService(typeof(TelegramBotClient));
            var notificationOptions = (TelegramOptions)services.GetService(typeof(TelegramOptions));

            if (tgClient is null) throw new ArgumentNullException(nameof(tgClient));

            var teleSink = new TeleSink(tgClient, new JsonFormatter(), notificationOptions.TelegramLogChatId, minimumLevel);

            return config.Sink(new PeriodicBatchingSink(teleSink, new PeriodicBatchingSinkOptions()));
        }
    }
}
