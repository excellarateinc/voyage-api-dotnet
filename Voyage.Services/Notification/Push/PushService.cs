using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR.Infrastructure;
using Voyage.Core;
using Voyage.Models;

namespace Voyage.Services.Notification.Push
{
    public class PushService : IPushService
    {
        private readonly IConnectionManager _connectionManager;

        public PushService(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager.ThrowIfNull(nameof(connectionManager));
        }

        public void PushNotification(NotificationModel model)
        {
            var context = _connectionManager.GetHubContext<NotificationHub>();

            if (string.IsNullOrEmpty(model.AssignedToUserId))
            {
                context.Clients.All.newNotification(model);
                return;
            }

            context.Clients.User(model.AssignedToUserId).newNotification(model);
        }

        public void PushChatMessage(IList<string> users, ChatMessageModel model)
        {
            var context = _connectionManager.GetHubContext<ChatHub>();

            if (!users.Any())
            {
                return;
            }

            context.Clients.Users(users).newChatMessage(model);
        }
    }
}
