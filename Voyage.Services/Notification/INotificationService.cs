using System.Collections.Generic;

namespace Voyage.Services.Notification
{
    public interface INotificationService
    {
        IEnumerable<Models.NotificationModel> GetNotifications(string userId);

        Models.NotificationModel CreateNotification(Models.NotificationModel notification);

        void MarkNotificationAsRead(string userId, int notificationId);

        void MarkAllNotificationsAsRead(string userId);
    }
}
