using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Models.NotificationModel> GetNotifications(string userId)
        {
            var notifications = _notificationRepository.GetAll()
                .Where(_ => _.AssignedToUserId == userId && !_.IsRead)
                .OrderByDescending(_ => _.CreatedDate);

            return _mapper.Map<IEnumerable<Models.NotificationModel>>(notifications);
        }

        public NotificationModel CreateNotification(NotificationModel notification)
        {
            var notificationEntity = _mapper.Map<Models.Entities.Notification>(notification);
            var model = _mapper.Map<Models.NotificationModel>(_notificationRepository.Add(notificationEntity));
            _pushService.PushNotification(model);
            return model;
        }

        public void MarkNotificationAsRead(string userId, int notificationId)
        {
            var notification = _notificationRepository.Get(notificationId);
            if (notification.AssignedToUserId != userId)
            {
                throw new UnauthorizedException();
            }

            notification.IsRead = true;
            _notificationRepository.SaveChanges();
        }

        public void MarkAllNotificationsAsRead(string userId)
        {
            var notifications = _notificationRepository.GetAll()
                .Where(_ => _.AssignedToUserId == userId && !_.IsRead);

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            _notificationRepository.SaveChanges();
        }
    }
}