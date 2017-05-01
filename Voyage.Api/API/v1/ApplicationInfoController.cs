using Voyage.Core;
using System.Web.Http;
using Voyage.Services.ApplicationInfo;

namespace Voyage.Api.API.V1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    [AllowAnonymous]
    public class ApplicationInfoController : ApiController
    {
        private readonly IApplicationInfoService _applicationInfoService;

        public ApplicationInfoController(IApplicationInfoService applicationInfoService)
        {
            _applicationInfoService = applicationInfoService.ThrowIfNull(nameof(applicationInfoService));
        }

        /**
        * @api {get} /v1/statuses Get application info
        * @apiVersion 0.1.0
        * @apiName GetStatuses
        * @apiGroup Status
        *
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
        [Route("statuses")]
        public IHttpActionResult Get()
        {
            var appInfo = _applicationInfoService.GetApplicationInfo();
            return Ok(appInfo);
        }
    }
}