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

        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            var result = _roleService.GetRoles();
            return Ok(result);
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

        [Route("role/claim")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClaim(RoleClaimModel model)
        {
            await _roleService.AddClaimAsync(model.Role, model.Claim);
            return StatusCode(HttpStatusCode.Created);
        }

    }
}
