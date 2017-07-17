using System.Collections.Generic;

namespace Voyage.Services.Notification
{
    public interface INotificationService
    {
        IEnumerable<Models.NotificationModel> GetNotifications(string userId);
    }
}
