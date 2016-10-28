using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API
{
    [Authorize]
    [RoutePrefix(Constants.RoutePrefixes.Role)]
    public class RoleController : ApiController
    {
        private IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService.ThrowIfNull(nameof(roleService));
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateRole(RoleModel model)
        {
            var result = await _roleService.CreateRoleAsync(model);
            if (result.Succeeded)
            {
                return StatusCode(HttpStatusCode.Created);
            }else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

    }
}
