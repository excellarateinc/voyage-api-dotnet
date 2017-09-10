using Voyage.Core;
using System.Web.Http;
using Voyage.Services.ApplicationInfo;
using Swashbuckle.Swagger.Annotations;
using Voyage.Models;

namespace Voyage.Api.API.V1
{
    /// <summary>
    /// Controller that provides information about the application.
    /// </summary>
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    [AllowAnonymous]
    public class ApplicationInfoController : ApiController
    {
        private readonly IApplicationInfoService _applicationInfoService;

        /// <summary>
        /// Constructor for the Application Info controller.
        /// </summary>
        public ApplicationInfoController(IApplicationInfoService applicationInfoService)
        {
            _applicationInfoService = applicationInfoService.ThrowIfNull(nameof(applicationInfoService));
        }

        /// <summary>
        /// Retrieves information about the application.
        /// </summary>
        [SwaggerResponse(200, "UserModel", typeof(ApplicationInfoModel))]
        [Route("statuses")]
        public IHttpActionResult Get()
        {
            var appInfo = _applicationInfoService.GetApplicationInfo();
            return Ok(appInfo);
        }
    }
}