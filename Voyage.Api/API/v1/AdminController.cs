using Voyage.Core;
using System.Web.Http;
using Voyage.Models;
using System.Threading.Tasks;
using Voyage.Api.Filters;
using Voyage.Services.Admin;

namespace Voyage.Api.API.V1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class AdminController : ApiController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService.ThrowIfNull(nameof(adminService));
        }

        /**
        * @api {post} /v1/users/{userId}/account-status Update users' account status
        * @apiVersion 1.0.0
        * @apiName ToggleAccountStatus
        * @apiGroup User
        * 
        * @apiPermission api.users.update
        * 
        * @apiParam {String} userId User ID
        * @apiParamExample {json} Request-Example:
        *       {
        *          "isActive": true,
        *          "IsVerifyRequired": false
        *       }
        * 
        * @apiUse UserSuccessModel
        * @apiUse BadRequestError
        **/
        [ClaimAuthorize(ClaimValue = AppClaims.UpdateUser)]
        [HttpPut]
        [Route("users/{userId}/account-status")]
        public async Task<IHttpActionResult> ToggleAccountStatus([FromUri] string userId, [FromBody] ChangeAccountStatusModel changeAccountStatusModel)
        {
            var result = await _adminService.ToggleAccountStatus(userId, changeAccountStatusModel);
            return Ok(result);
        }
    }
}