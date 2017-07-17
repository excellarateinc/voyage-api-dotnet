using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Voyage.Core;
using Voyage.Data.Repositories.Notification;

namespace Voyage.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository.ThrowIfNull(nameof(notificationRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public IEnumerable<Models.NotificationModel> GetNotifications(string userId)
        {
            var notifications = _notificationRepository.GetAll()
                .Where(_ => _.AssignedToUserId == userId);

            return _mapper.Map<IEnumerable<Models.NotificationModel>>(notifications);
        }
    }
}