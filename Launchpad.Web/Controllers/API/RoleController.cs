using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.Web.Filters;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.Controllers.API
{
    [Authorize]
    [RoutePrefix(RoutePrefixes.Role)]
    public class RoleController : ApiController
    {
        private IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService.ThrowIfNull(nameof(roleService));
        }

        [ClaimAuthorize(ClaimValue = LssClaims.ListRoles)]
        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            var result = _roleService.GetRoles();
            return Ok(result);
        }

        [ClaimAuthorize(ClaimValue = LssClaims.CreateRole)]
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

        [ClaimAuthorize(ClaimValue = LssClaims.CreateClaim)]
        [Route("role/claim")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClaim(RoleClaimModel model)
        {
            await _roleService.AddClaimAsync(model.Role, model.Claim);
            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPost]
        [Route("role/claim")]
        public IHttpActionResult RemoveClaim(RoleClaimModel model)
        {
           _roleService.RemoveClaim(model.Role, model.Claim);
            return StatusCode(HttpStatusCode.NoContent);
        }


        [HttpPost]
        public async Task<IHttpActionResult> RemoveRole(RoleModel role)
        {
            var result = await _roleService.RemoveRoleAsync(role);
            if (result.Succeeded)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return BadRequest();
        }



    }
}
