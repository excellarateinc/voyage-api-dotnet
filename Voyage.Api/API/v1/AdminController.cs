using Voyage.Core;
using System.Web.Http;
using Voyage.Services.User;
using Voyage.Models;
using System.Threading.Tasks;
using System;
using Voyage.Api.Filters;

namespace Voyage.Api.API.V1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class AdminController : ApiController
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));
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
        [HttpPost]
        [Route("users/{userId}/account-status")]
        public async Task<IHttpActionResult> ToggleAccountStatus([FromUri] string userId, [FromBody] ChangeAccountStatusModel changeAccountStatusModel)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();
            try
            {
                var user = await _userService.GetUserAsync(userId);
                user.IsActive = changeAccountStatusModel.IsActive == null ? user.IsActive : (bool)changeAccountStatusModel.IsActive;
                user.IsVerifyRequired = changeAccountStatusModel.IsVerifyRequired == null ? user.IsVerifyRequired : (bool)changeAccountStatusModel.IsVerifyRequired;
                var result = await _userService.UpdateUserAsync(userId, user);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}