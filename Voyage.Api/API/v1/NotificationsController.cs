using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;
using Voyage.Api.Filters;
using Voyage.Core;
using Voyage.Models;
using Voyage.Services.Notification;
using Voyage.Services.User;

namespace Voyage.Api.API.v1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class NotificationsController : ApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService.ThrowIfNull(nameof(notificationService));
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
        public async Task<IHttpActionResult> GetNotifications()
        {
            return Ok();
        }

    }
}
