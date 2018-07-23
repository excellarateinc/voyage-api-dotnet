using Voyage.Core;
using Voyage.Models;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Api.UserManager.Filters;
using Voyage.Services.Role;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;

namespace Voyage.Api.UserManager.API.V1
{
    /// <summary>
    /// Controller that handles role management.
    /// </summary>
    [RoutePrefix(RoutePrefixConstants.RoutePrefixes.V1)]
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;

        /// <summary>
        /// Contructor for the Role Controller.
        /// </summary>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService.ThrowIfNull(nameof(roleService));
        }

        /// <summary>
        /// Get a role
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.ViewRole)]
        [HttpGet]
        [Route("roles/{roleId}", Name = "GetRoleById")]
        [SwaggerResponse(200, "RoleModel", typeof(RoleModel))]
        [SwaggerResponse(401, "UnauthorizedException")]
        [SwaggerResponse(404, "NotFoundException")]
        public IHttpActionResult GetRoleById(string roleId)
        {
            var result = _roleService.GetRoleById(roleId);
            return Ok(result);
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.ListRoles)]
        [HttpGet]
        [Route("roles")]
        [SwaggerResponse(200, "IEnumerable<RoleModel>", typeof(IEnumerable<RoleModel>))]
        [SwaggerResponse(401, "UnauthorizedException")]
        public IHttpActionResult GetRoles()
        {
            var result = _roleService.GetRoles();
            return Ok(result);
        }

        /// <summary>
        /// Create a role
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.CreateRole)]
        [HttpPost]
        [Route("roles")]
        [SwaggerResponse(201, "RoleModel", typeof(RoleModel))]
        [SwaggerResponse(400, "BadRequestException")]
        [SwaggerResponse(401, "UnauthorizedException")]
        public async Task<IHttpActionResult> CreateRole(RoleModel model)
        {
            var result = await _roleService.CreateRoleAsync(model);
            return CreatedAtRoute("GetRoleById", new { roleId = result.Id }, result);
        }

        /// <summary>
        /// Create a role permission
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.AddRolePermission)]        
        [HttpPost]
        [Route("roles/{roleId}/permissions")]
        [SwaggerResponse(201, "ClaimModel", typeof(ClaimModel))]
        [SwaggerResponse(400, "BadRequestException")]
        [SwaggerResponse(401, "UnauthorizedException")]
        public async Task<IHttpActionResult> AddClaim([FromUri] string roleId, ClaimModel permission)
        {
            var result = await _roleService.AddClaimAsync(roleId, permission);
            var actionResult = CreatedAtRoute(
                "GetClaimById",
                new
                {
                    RoleId = roleId,
                    ClaimId = result.Id
                },
                result);
            return actionResult;
        }

        /// <summary>
        /// Get a Permission
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.GetRolePermission)]
        [HttpGet]
        [Route("roles/{roleId}/permissions/{permissionId}", Name = "GetPermissionById")]
        [SwaggerResponse(200, "ClaimModel", typeof(ClaimModel))]
        [SwaggerResponse(401, "UnauthorizedException")]
        [SwaggerResponse(404, "NotFoundException")]
        public async Task<IHttpActionResult> GetClaimById(string roleId, int permissionId)
        {
            var result = await _roleService.GetClaimById(roleId, permissionId);
            return Ok(result);
        }

        /// <summary>
        /// Remove a role permission
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.DeleteRolePermission)]
        [HttpDelete]
        [Route("roles/{roleId}/permissions/{permissionId}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(401, "UnauthorizedException")]
        public async Task<IHttpActionResult> RemoveClaim(string roleId, int permissionId)
        {
            await _roleService.RemoveClaim(roleId, permissionId);
            return Ok();
        }

        /// <summary>
        /// Get role permissions
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.ListRolePermission)]
        [HttpGet]
        [Route("roles/{roleId}/permissions")]
        [SwaggerResponse(200, "IEnumerable<ClaimModel>", typeof(IEnumerable<ClaimModel>))]
        [SwaggerResponse(401, "UnauthorizedException")]
        public IHttpActionResult GetClaims(string roleId)
        {
            var result = _roleService.GetRoleClaimsByRoleId(roleId);
            return Ok(result);
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.DeleteRole)]
        [HttpDelete]        
        [Route("roles/{roleId}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(401, "UnauthorizedException")]
        [SwaggerResponse(404, "NotFoundException")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string roleId)
        {
            await _roleService.RemoveRoleAsync(roleId);
            return Ok();
        }
    }
}
