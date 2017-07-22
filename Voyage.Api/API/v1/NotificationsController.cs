using System.IdentityModel.Claims;
using System.Web.Http;
using Microsoft.Owin.Security;
using Voyage.Api.Filters;
using Voyage.Core;
using Voyage.Models;
using Voyage.Services.Notification;

namespace Voyage.Api.API.v1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class NotificationsController : ApiController
    {
        private readonly INotificationService _notificationService;
        private readonly IAuthenticationManager _authenticationManager;

        public NotificationsController(INotificationService notificationService, IAuthenticationManager authenticationManager)
        {
            _notificationService = notificationService.ThrowIfNull(nameof(notificationService));
            _authenticationManager = authenticationManager.ThrowIfNull(nameof(authenticationManager));
        }

        /**
        * @api {get} v1/notifications Get notifications
        * @apiVersion 1.0.0
        * @apiName GetNotifications
        * @apiGroup Notifications
        * @apiSampleRequest http://qa-api-ms.voyageframework.com/api/v1/notifications
        * @apiSuccess {String} version Version Number
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       {
        *           "buildNumber": "some_number"
        *       }
        *   ]
        **/
        [ClaimAuthorize(ClaimValue = AppClaims.ListNotifications)]
        [HttpGet]
        [Route("notifications")]
        public IHttpActionResult GetNotifications()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var notifications = _notificationService.GetNotifications(userId);
            return Ok(notifications);
        }

        [ClaimAuthorize(ClaimValue = AppClaims.MarkNotificationsAsRead)]
        [HttpPut]
        [Route("notifications/{id}")]
        public IHttpActionResult MarkNotificationAsRead(int id)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            _notificationService.MarkNotificationAsRead(userId, id);
            return Ok(new { });
        }

        [ClaimAuthorize(ClaimValue = AppClaims.MarkNotificationsAsRead)]
        [HttpPut]
        [Route("notifications")]
        public IHttpActionResult MarkAllNotificationsAsRead()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            _notificationService.MarkAllNotificationsAsRead(userId);
            return Ok(new { });
        }

        [ClaimAuthorize(ClaimValue = AppClaims.CreateNotification)]
        [HttpPost]
        [Route("notifications")]
        public IHttpActionResult CreateNotification(NotificationModel notification)
        {
            var createdNotification = _notificationService.CreateNotification(notification);
            return Ok(createdNotification);
        }
    }
}
