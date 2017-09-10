using System.IdentityModel.Claims;
using System.Web.Http;
using Microsoft.Owin.Security;
using Voyage.Api.Filters;
using Voyage.Core;
using Voyage.Models;
using Voyage.Services.Notification;
using System.Collections.Generic;
using Swashbuckle.Swagger.Annotations;

namespace Voyage.Api.API.v1
{
    /// <summary>
    /// Controller that handles application notifications.
    /// </summary>
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class NotificationsController : ApiController
    {
        private readonly INotificationService _notificationService;
        private readonly IAuthenticationManager _authenticationManager;

        /// <summary>
        /// Constructor for the Notifications Controller.
        /// </summary>
        public NotificationsController(INotificationService notificationService, IAuthenticationManager authenticationManager)
        {
            _notificationService = notificationService.ThrowIfNull(nameof(notificationService));
            _authenticationManager = authenticationManager.ThrowIfNull(nameof(authenticationManager));
        }

        /// <summary>
        /// Get all notifications for the current user
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.ListNotifications)]
        [HttpGet]
        [Route("notifications")]
        [SwaggerResponse(200, "IEnumerable<UserModel>", typeof(IEnumerable<NotificationModel>))]
        [SwaggerResponse(401, "UnauthorizedException")]
        public IHttpActionResult GetNotifications()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var notifications = _notificationService.GetNotifications(userId);
            return Ok(notifications);
        }

        /// <summary>
        /// Mark a single notification as read
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.MarkNotificationsAsRead)]
        [HttpPut]
        [Route("notifications/{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(401, "UnauthorizedException")]
        public IHttpActionResult MarkNotificationAsRead(int id)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            _notificationService.MarkNotificationAsRead(userId, id);
            return Ok(new { });
        }

        /// <summary>
        /// Mark all nofications as read
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.MarkNotificationsAsRead)]
        [HttpPut]
        [Route("notifications")]
        [SwaggerResponse(200)]
        [SwaggerResponse(401, "UnauthorizedException")]
        public IHttpActionResult MarkAllNotificationsAsRead()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            _notificationService.MarkAllNotificationsAsRead(userId);
            return Ok(new { });
        }

        /// <summary>
        /// Create a notification
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.CreateNotification)]
        [HttpPost]
        [Route("notifications")]
        [SwaggerResponse(200, "NotificationModel", typeof(NotificationModel))]
        [SwaggerResponse(401, "UnauthorizedException")]
        public IHttpActionResult CreateNotification(NotificationModel notification)
        {
            var createdNotification = _notificationService.CreateNotification(notification);
            return Ok(createdNotification);
        }
    }
}
