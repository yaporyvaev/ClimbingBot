using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClimbingBot.Abstractions;
using ClimbingBot.Entities;
using ClimbingBot.Notifications;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ClimbingBot.Telegram.Handlers
{
    public class CreatePoolNotificationHandler : INotificationHandler<OnPollCreatedNotification>
    {
        private readonly TelegramBotClient _tgClient;
        private readonly IRepository<TelegramGroup> _groupRepository;
        private readonly IRepository<PollHistory> _historyRepository;

        public CreatePoolNotificationHandler(
            TelegramBotClient tgClient, 
            IRepository<TelegramGroup> groupRepository, 
            IRepository<PollHistory> historyRepository)
        {
            _tgClient = tgClient;
            _groupRepository = groupRepository;
            _historyRepository = historyRepository;
        }

        public async Task Handle(OnPollCreatedNotification notification, CancellationToken cancellationToken)
        {
            var chatIds = notification.ChatId.HasValue
                ? new[] {notification.ChatId.Value}
                : _groupRepository.GetAll().Select(g => g.GroupId).ToArray();
            
            foreach (var chatId in chatIds)
            {
                var now = DateTime.Now.Date;
                var recentMessage = _historyRepository.GetAll()
                    .FirstOrDefault(h => h.GroupId == chatId && h.CreatedAt.Date == now);

                if (recentMessage != null)
                {
                    try
                    {
                        await _tgClient.ForwardMessageAsync(new ChatId(chatId), new ChatId(chatId), recentMessage.MessageId,
                            false, cancellationToken: cancellationToken);
                    }
                    catch (ApiRequestException requestException)
                    {
                        if (requestException.ErrorCode == 400)
                        {
                            var poolMessage = await _tgClient.SendPollAsync(new ChatId(chatId), notification.Poll.Title, notification.Poll.Options,false,
                                PollType.Regular,false, disableNotification:false, cancellationToken: cancellationToken);
                            
                            recentMessage.MessageId = poolMessage.MessageId;
                            recentMessage.CreatedAt = DateTime.Now;
                            await _historyRepository.Update(recentMessage);
                        }
                    }
                }
                else
                {
                    var poolMessage = await _tgClient.SendPollAsync(new ChatId(chatId), notification.Poll.Title, notification.Poll.Options,false,
                        PollType.Regular,false, disableNotification:false, cancellationToken: cancellationToken);
                
                    await _historyRepository.Add(new PollHistory
                    {
                        CreatedAt = DateTime.Now,
                        GroupId = chatId,
                        MessageId = poolMessage.MessageId
                    });
                }
            }
        }
    }
}