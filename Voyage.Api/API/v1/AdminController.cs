using Voyage.Core;
using System.Web.Http;
using Voyage.Models;
using System.Threading.Tasks;
using Voyage.Api.Filters;
using Voyage.Services.Admin;
using Swashbuckle.Swagger.Annotations;

namespace Voyage.Api.API.V1
{
    /// <summary>
    /// Admin Constructor
    /// </summary>
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class AdminController : ApiController
    {
        private readonly IAdminService _adminService;

        /// <summary>
        /// AdminConstroller Constructor
        /// </summary>
        /// <param name="adminService"></param>
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService.ThrowIfNull(nameof(adminService));
        }

        /// <summary>
        /// Update users' account status
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="changeAccountStatusModel"></param>
        /// <returns>A user record with an HTTP 200, or null with an HTTP 400.</returns>
        [ClaimAuthorize(ClaimValue = AppClaims.UpdateUser)]
        [HttpPut]
        [Route("users/{userId}/account-status")]
        [SwaggerResponse(200, "UserModel", typeof(UserModel))]
        [SwaggerResponse(400, "BadRequestException")]
        [SwaggerResponse(401, "UnauthorizedException")]
        public async Task<IHttpActionResult> ToggleAccountStatus([FromUri] string userId, [FromBody] ChangeAccountStatusModel changeAccountStatusModel)
        {
            var result = await _adminService.ToggleAccountStatus(userId, changeAccountStatusModel);
            return Ok(result);
        }
    }
}