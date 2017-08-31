using Voyage.Core;
using System.Web.Http;
using Voyage.Services.ApplicationInfo;
using Swashbuckle.Swagger.Annotations;
using Voyage.Models;

namespace Voyage.Api.API.V1
{
    /// <summary>
    /// Application Info Controller
    /// </summary>
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    [AllowAnonymous]
    public class ApplicationInfoController : ApiController
    {
        private readonly IApplicationInfoService _applicationInfoService;

        /// <summary>
        /// ApplicationInfoConstructorController
        /// </summary>
        /// <param name="applicationInfoService"></param>
        public ApplicationInfoController(IApplicationInfoService applicationInfoService)
        {
            _applicationInfoService = applicationInfoService.ThrowIfNull(nameof(applicationInfoService));
        }

        /// <summary>
        /// Get application info
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(200, "UserModel", typeof(ApplicationInfoModel))]
        [Route("statuses")]
        public IHttpActionResult Get()
        {
            var appInfo = _applicationInfoService.GetApplicationInfo();
            return Ok(appInfo);
        }
    }
}