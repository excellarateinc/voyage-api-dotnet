using System.Collections.Generic;

namespace Voyage.Services.Notification
{
    public interface INotificationService
    {
        IEnumerable<Models.NotificationModel> GetNotifications(string userId);

        void MarkNotificationAsRead(string userId, int notificationId);

        void MarkAllNotificationsAsRead(string userId);
    }
}
