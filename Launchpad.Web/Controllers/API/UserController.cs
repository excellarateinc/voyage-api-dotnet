using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.Web.Extensions;
using Launchpad.Web.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.Controllers.API
{
    [Authorize]
    [RoutePrefix(Constants.RoutePrefixes.User)]
    public class UserController : ApiController
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));

        }

        [ClaimAuthorize(ClaimValue = LssClaims.ListUsers)]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        [ClaimAuthorize(ClaimValue =LssClaims.ListUserClaims)]
        [HttpGet]
        [Route("user/claims")]
        public IHttpActionResult GetClaims()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var clientFacingClaims = claimsIdentity.Claims
                .Where(_=>_.Type == Constants.LssClaims.Type)
                .Select(_ => new ClaimModel { ClaimType = _.Type, ClaimValue = _.Value })
                .ToList();
            return Ok(clientFacingClaims);
        }

        [ClaimAuthorize(ClaimValue = LssClaims.AssignRole)]
        [HttpPost]
        [Route("user/assign")]
        public async Task<IHttpActionResult> AssignRole(UserRoleModel userRole)
        {
            var identityResult = await _userService.AssignUserRoleAsync(userRole.Role, userRole.User);

            if (!identityResult.Succeeded)
            {
                return StatusCode(System.Net.HttpStatusCode.BadRequest);
            }

            return Ok();
        }

        [HttpPost]
        [Route("user/revoke")]
        public async Task<IHttpActionResult> RemoveRole(UserRoleModel model)
        {
            var result = await _userService.RemoveUserFromRoleAsync(model.Role, model.User);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddErrors(result);
                if (ModelState.IsValid)
                    return BadRequest();
                else
                    return BadRequest(ModelState);
            }
        }
    }
}
