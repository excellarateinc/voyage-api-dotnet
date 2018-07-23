using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.Chat;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.Notification;
using Voyage.Services.Notification.Push;
using Voyage.Services.User;

namespace Voyage.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IChatChannelRepository _chatChannelRepository;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IChatChannelMemberRepository _chatChannelMemberRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IPushService _pushService;

        public ChatService(
            IChatChannelRepository chatChannelRepository,
            IChatMessageRepository chatMessageRepository,
            IChatChannelMemberRepository chatChannelMemberRepository,
            IMapper mapper,
            IUserService userService,
            INotificationService notificationService,
            IPushService pushService)
        {
            _chatChannelRepository = chatChannelRepository.ThrowIfNull(nameof(chatChannelRepository));
            _chatMessageRepository = chatMessageRepository.ThrowIfNull(nameof(chatMessageRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _userService = userService.ThrowIfNull(nameof(userService));
            _chatChannelMemberRepository = chatChannelMemberRepository.ThrowIfNull(nameof(chatChannelMemberRepository));
            _notificationService = notificationService.ThrowIfNull(nameof(notificationService));
            _pushService = pushService.ThrowIfNull(nameof(pushService));
        }

        public IEnumerable<ChatChannelModel> GetChannels(string userId)
        {
            var channels = from channel in _chatChannelRepository.GetAll()
                           join member in _chatChannelMemberRepository.GetAll() on channel.ChannelId equals member.ChannelId
                           where member.UserId == userId
                           orderby channel.CreateDate descending
                           select channel;

            return _mapper.Map<IEnumerable<ChatChannelModel>>(channels.ToList());
        }

        public async Task<ChatChannelModel> CreateChannel(string userId)
        {
            var user = await _userService.GetUserAsync(userId);
            var channel = new ChatChannel
            {
                Name = user.Username,
                CreatedBy = userId,
                CreateDate = DateTime.Now
            };
            await _chatChannelRepository.AddAsync(channel);

            await _chatChannelMemberRepository.AddAsync(new ChatChannelMember
            {
                ChannelId = channel.ChannelId,
                UserId = userId,
                CreateDate = DateTime.UtcNow
            });

            // Add support to the channel.
            var support = await _userService.GetUserByNameAsync("CustomerSupport");
            if (support.Id != userId)
            {
                await _chatChannelMemberRepository.AddAsync(new ChatChannelMember
                {
                    ChannelId = channel.ChannelId,
                    UserId = support.Id,
                    CreateDate = DateTime.UtcNow
                });
            }

            return _mapper.Map<ChatChannelModel>(channel);
        }

        public IEnumerable<ChatMessageModel> GetMessagesByChannel(string userId, int channelId)
        {
            var channel = (from c in _chatChannelRepository.GetAll()
                           join member in _chatChannelMemberRepository.GetAll() on c.ChannelId equals member.ChannelId
                           where member.UserId == userId && c.ChannelId == channelId
                           select c).FirstOrDefault();

            if (channel == null)
            {
                throw new NotFoundException("Channel not found or user doesn't have access.");
            }

            var messages = _chatMessageRepository.GetAll()
                .Where(_ => _.ChannelId == channelId)
                .ToList();

            return _mapper.Map<IEnumerable<ChatMessageModel>>(messages);
        }

        public async Task<ChatMessageModel> CreateMessage(string userId, ChatMessageModel model)
        {
            var members = _chatChannelMemberRepository.GetAll()
                .Where(_ => _.ChannelId == model.ChannelId);

            if (!members.Any(_ => _.UserId == userId))
            {
                throw new NotFoundException("Channel not found or user doesn't have access.");
            }

            var user = await _userService.GetUserAsync(userId);

            model.Username = user.Username;
            model.CreatedBy = user.Id;
            model.CreateDate = DateTime.UtcNow;

            var message = _mapper.Map<ChatMessage>(model);
            await _chatMessageRepository.AddAsync(message);

            var createdMessage = _mapper.Map<ChatMessageModel>(message);

            var otherMembers = members.Where(_ => _.UserId != userId)
                .Select(_ => _.UserId).ToList();

            foreach (var member in otherMembers)
            {
                await _notificationService.CreateNotification(new NotificationModel
                {
                    Subject = $"New message from {user.Username}",
                    Description = $"{createdMessage.Message}",
                    CreatedBy = user.Id,
                    AssignedToUserId = member,
                    CreatedDate = DateTime.UtcNow,
                    IsRead = false
                });
            }

            _pushService.PushChatMessage(otherMembers, createdMessage);

            return createdMessage;
        }
    }
}