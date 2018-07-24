using System.Collections.Generic;
using System.Threading.Tasks;

namespace Voyage.Services.Notification
{
    public interface INotificationService
    {
        Task<IEnumerable<Models.NotificationModel>> GetNotifications(string userId);

        Task<Models.NotificationModel> CreateNotification(Models.NotificationModel notification);

        Task MarkNotificationAsRead(string userId, int notificationId);

        Task MarkAllNotificationsAsRead(string userId);
    }
}
