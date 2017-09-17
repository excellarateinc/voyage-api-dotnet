using System.Collections.Generic;
using Voyage.Models;

namespace Voyage.Services.Notification.Push
{
    public interface IPushService
    {
        void PushNotification(NotificationModel model);

        void PushChatMessage(IList<string> users, ChatMessageModel model);
    }
}
