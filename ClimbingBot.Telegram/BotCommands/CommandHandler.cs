using System;
using System.Linq;
using System.Threading.Tasks;
using ClimbingBot.Telegram.Exceptions;
using ClimbingBot.Telegram.Extensions;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ClimbingBot.Telegram.BotCommands
{
    public class CommandHandler
    {
        private readonly CommandStateStore _stateStore;
        private readonly TelegramBotClient _tgClient;
        private readonly CommandFactory _commandFactory;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(
            CommandStateStore stateStore, 
            TelegramBotClient tgClient, 
            CommandFactory commandFactory,
            ILogger<CommandHandler> logger)
        {
            _stateStore = stateStore;
            _tgClient = tgClient;
            _commandFactory = commandFactory;
            _logger = logger;
        }

        public async Task Handle(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                await HandleBotMessageCommand(update.Message);
            }
        }

        private async Task HandleBotMessageCommand(Message message)
        {
            var messageSenderId = message.From!.Id;

            var messagePayload = string.Join(" ", message.Text!.Split(" ").Skip(1));//todo refactor this shit
            var commandType = message.Text.Split("@").FirstOrDefault();

            var command = _commandFactory.Create(commandType);
            if(command == null) return;

            var chatId = message.Chat.Id;
            try
            {
                var state = await command.Handle(message, messagePayload);
                
                if (state != null)
                {
                    await _tgClient.SendTextMessage(chatId, state.BuildMessage());
                }
            }
            catch (BotCommandException commandException)
            {
                await _tgClient.SendTextMessage(chatId, commandException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Command handling failed");
                await _tgClient.SendTextMessage(chatId, "Command handling failed. It was canceled.");
                _stateStore.Reset(messageSenderId);
            }
        }
    }
}