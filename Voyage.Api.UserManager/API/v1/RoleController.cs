using System.Net;
using System.Net.Http;
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
    [RoutePrefix(RoutePrefixConstants.RoutePrefixes.V1)]
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService.ThrowIfNull(nameof(roleService));
        }

        /// <summary>
        /// Get a role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [SwaggerResponse(200, "RoleModel", typeof(RoleModel))]
        [SwaggerResponse(401, "UnauthorizedException")]
        [SwaggerResponse(404, "NotFoundException")]
        [ClaimAuthorize(ClaimValue = AppClaims.ViewRole)]
        [HttpGet]
        [Route("roles/{roleId}", Name = "GetRoleById")]
        public IHttpActionResult GetRoleById(string roleId)
        {
            var result = _roleService.GetRoleById(roleId);
            return Ok(result);
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(200, "IEnumerable<RoleModel>", typeof(IEnumerable<RoleModel>))]
        [SwaggerResponse(401, "UnauthorizedException")]
        [ClaimAuthorize(ClaimValue = AppClaims.ListRoles)]
        [HttpGet]
        [Route("roles")]
        public IHttpActionResult GetRoles()
        {
            var result = _roleService.GetRoles();
            return Ok(result);
        }

        /// <summary>
        /// Create a role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SwaggerResponse(201, "RoleModel", typeof(RoleModel))]
        [SwaggerResponse(400, "BadRequestException")]
        [SwaggerResponse(401, "UnauthorizedException")]
        [ClaimAuthorize(ClaimValue = AppClaims.CreateRole)]
        [HttpPost]
        [Route("roles")]
        public async Task<IHttpActionResult> CreateRole(RoleModel model)
        {
            var result = await _roleService.CreateRoleAsync(model);
            return CreatedAtRoute("GetRoleById", new { roleId = result.Id }, result);
        }

        /// <summary>
        /// Create a role permission
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        [SwaggerResponse(201, "ClaimModel", typeof(ClaimModel))]
        [SwaggerResponse(400, "BadRequestException")]
        [SwaggerResponse(401, "UnauthorizedException")]
        [ClaimAuthorize(ClaimValue = AppClaims.AddRolePermission)]
        [Route("roles/{roleId}/permissions")]
        [HttpPost]
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
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [SwaggerResponse(200, "ClaimModel", typeof(ClaimModel))]
        [SwaggerResponse(401, "UnauthorizedException")]
        [SwaggerResponse(404, "NotFoundException")]
        [ClaimAuthorize(ClaimValue = AppClaims.GetRolePermission)]
        [Route("roles/{roleId}/permissions/{permissionId}", Name = "GetPermissionById")]
        [HttpGet]
        public IHttpActionResult GetClaimById(string roleId, int permissionId)
        {
            var result = _roleService.GetClaimById(roleId, permissionId);
            return Ok(result);
        }

        /// <summary>
        /// Remove a role permission
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [SwaggerResponse(200)]
        [SwaggerResponse(401, "UnauthorizedException")]
        [ClaimAuthorize(ClaimValue = AppClaims.DeleteRolePermission)]
        [HttpDelete]
        [Route("roles/{roleId}/permissions/{permissionId}")]
        public IHttpActionResult RemoveClaim(string roleId, int permissionId)
        {
            _roleService.RemoveClaim(roleId, permissionId);
            return Ok();
        }

        /// <summary>
        /// Get role permissions
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [SwaggerResponse(200, "IEnumerable<ClaimModel>", typeof(IEnumerable<ClaimModel>))]
        [SwaggerResponse(401, "UnauthorizedException")]
        [ClaimAuthorize(ClaimValue = AppClaims.ListRolePermission)]
        [HttpGet]
        [Route("roles/{roleId}/permissions")]
        public IHttpActionResult GetClaims(string roleId)
        {
            var result = _roleService.GetRoleClaimsByRoleId(roleId);
            return Ok(result);
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ClaimAuthorize(ClaimValue = AppClaims.DeleteRole)]
        [Route("roles/{roleId}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(401, "UnauthorizedException")]
        [SwaggerResponse(404, "NotFoundException")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string roleId)
        {
            await _roleService.RemoveRoleAsync(roleId);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }
    }
}
