using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.Notification;
using Voyage.Models;
using Voyage.Services.Notification.Push;

namespace Voyage.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IPushService _pushService;

        public NotificationService(
            INotificationRepository notificationRepository,
            IMapper mapper,
            IPushService pushService)
        {
            _notificationRepository = notificationRepository.ThrowIfNull(nameof(notificationRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _pushService = pushService.ThrowIfNull(nameof(pushService));
        }

        public async Task<IEnumerable<Models.NotificationModel>> GetNotifications(string userId)
        {
            var notifications = await _notificationRepository.GetAll()
                .Where(_ => _.AssignedToUserId == userId && !_.IsRead)
                .OrderByDescending(_ => _.CreatedDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<Models.NotificationModel>>(notifications);
        }

        public async Task<NotificationModel> CreateNotification(NotificationModel notification)
        {
            var notificationEntity = _mapper.Map<Models.Entities.Notification>(notification);
            var model = _mapper.Map<Models.NotificationModel>(await _notificationRepository.AddAsync(notificationEntity));
            _pushService.PushNotification(model);
            return model;
        }

        public async Task MarkNotificationAsRead(string userId, int notificationId)
        {
            var notification = await _notificationRepository.GetAsync(notificationId);
            if (notification.AssignedToUserId != userId)
            {
                throw new UnauthorizedException();
            }

            notification.IsRead = true;
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task MarkAllNotificationsAsRead(string userId)
        {
            var notifications = _notificationRepository.GetAll()
                .Where(_ => _.AssignedToUserId == userId && !_.IsRead);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _notificationRepository.SaveChangesAsync();
        }
    }
}