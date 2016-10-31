using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API
{
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

        [HttpPost]
        [Route("user/assign")]
        public async Task<IHttpActionResult> AssignRole(UserRoleModel userRole)
        {
            var identityResult = await _userService.AssignUserRoleAsync(userRole.Role, userRole.User);

            if (identityResult.Succeeded)
            {
                _userService.ConfigureUserClaims(userRole.User);
            }
            else
            {
                return StatusCode(System.Net.HttpStatusCode.BadRequest);
            }

            return Ok();
        }
    }
}
