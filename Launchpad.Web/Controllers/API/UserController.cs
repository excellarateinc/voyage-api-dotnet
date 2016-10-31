using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

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

        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

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
    }
}
