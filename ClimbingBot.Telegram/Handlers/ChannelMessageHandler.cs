using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ClimbingBot.Telegram.BotCommands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ClimbingBot.Telegram.Handlers
{
    public class ChannelMessageHandler : IHostedService
    {
        private readonly TelegramBotClient _tgClient;
        private readonly CommandHandler _commandHandler;
        private readonly ILogger<ChannelMessageHandler> _logger;
        private CancellationTokenSource _cts;
        
        private string _botUserName;
        
        public ChannelMessageHandler(
            ILogger<ChannelMessageHandler> logger,
            TelegramBotClient tgClient,
            CommandHandler commandHandler)
        {
            _logger = logger;
            _tgClient = tgClient;
            _commandHandler = commandHandler;
        }

        #region IHostedService
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();
            _botUserName = _tgClient.GetMeAsync().GetAwaiter().GetResult().Username;

            _tgClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message }, Offset = -1 },
                _cts.Token);
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();

            return Task.CompletedTask;
        }
        #endregion

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
        {
            if(string.IsNullOrEmpty(update.Message.Text)) return;
            
            await _commandHandler.Handle(update);
        }
        
        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(exception, errorMessage);

            return Task.CompletedTask;
        }
    }
}
