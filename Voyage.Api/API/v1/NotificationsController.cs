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
        * @api {get} /v1/notifications Get all notifications for current user
        * @apiVersion 1.0.0
        * @apiName GetNotifications
        * @apiGroup Notifications
        *
        * @apiPermission api.notifications.list
        *
        * @apiUse AuthHeader
        * @apiSampleRequest http://qa-api-ms.voyageframework.com/api/v1/notifications
        * @apiSuccess {Object[]} notifications List of notifications
        * @apiSuccess {String} notifications.subject Subject of the notification
        * @apiSuccess {String} notifications.description Description of the notification
        * @apiSuccess {String} notifications assignedToUserId UserId the notification is assigned to
        * @apiSuccess {String} notifications.isRead Flag indicating if the notification has been marked read
        * @apiSuccess {String} notifications.createdBy UserId the notification was created by
        * @apiSuccess {String} notifications.createdDate Date the notification was created on
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *      {
        *        "id": 1,
        *        "subject": "Test Notification",
        *        "description": "This is a test for notifications",
        *        "assignedToUserId": "fb9f65d2-699c-4f08-a2e4-8e6c28190a84",
        *        "isRead": false,
        *        "createdBy": "fb9f65d2-699c-4f08-a2e4-8e6c28190a84",
        *        "createdDate": "2017-07-22T16:45:34.1766667"
        *       }
        *   ]
        *
        * @apiUse UnauthorizedError
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

        /// <summary>
        /// TODO: Holding off on ApiDoc until we determine API documentation strategy going forward.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ClaimAuthorize(ClaimValue = AppClaims.MarkNotificationsAsRead)]
        [HttpPut]
        [Route("notifications/{id}")]
        public IHttpActionResult MarkNotificationAsRead(int id)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            _notificationService.MarkNotificationAsRead(userId, id);
            return Ok(new { });
        }

        /// <summary>
        /// TODO: Holding off on ApiDoc until we determine API documentation strategy going forward.
        /// </summary>
        /// <returns></returns>
        [ClaimAuthorize(ClaimValue = AppClaims.MarkNotificationsAsRead)]
        [HttpPut]
        [Route("notifications")]
        public IHttpActionResult MarkAllNotificationsAsRead()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            _notificationService.MarkAllNotificationsAsRead(userId);
            return Ok(new { });
        }

        /// <summary>
        /// TODO: Holding off on ApiDoc until we determine API documentation strategy going forward.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
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
