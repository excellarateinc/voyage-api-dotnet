using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
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
        public IHttpActionResult AssignRole(UserRoleModel userRole)
        {
            return Ok();
        }
    }
}
