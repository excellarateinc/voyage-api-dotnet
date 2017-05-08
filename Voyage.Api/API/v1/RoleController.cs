using System.Net;
using System.Net.Http;
using Voyage.Core;
using Voyage.Models;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Api.Filters;
using Voyage.Services.Role;

namespace Voyage.Api.API.V1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService.ThrowIfNull(nameof(roleService));
        }

        /**
        * @api {get} /v1/roles/:roleId Get a role
        * @apiVersion 1.0.0
        * @apiName GetRoleById
        * @apiGroup Role
        *
        * @apiPermission app.permission->view.role
        *
        * @apiUse AuthHeader
        *
        * @apiParam {String} roleId Role ID
        *
        * @apiSuccess {Object} role
        * @apiSuccess {String} role.id Role ID
        * @apiSuccess {String} role.name Name of the role
        * @apiSuccess {Object[]} role.permissions Permissions associated to the role
        * @apiSuccess {String} role.permissions.permissionType Type of the permission
        * @apiSuccess {String} role.permissions.permissionValue Value of the permission
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *      {
        *           "id": "76d216ab-cb48-4c5f-a4ba-1e9c3bae1fe6",
        *           "name": "New Role 1",
        *           "permissions": [
        *               {
        *                   permissionType: "app.permission",
        *                   permissionValue: "view.role"
        *               }
        *           ]
        *       }
        *   ]
        *
        * @apiUse UnauthorizedError
        * @apiUse NotFoundError
        **/
        [PermissionAuthorize(PermissionValue = Constants.AppPermissions.ViewRole)]
        [HttpGet]
        [Route("roles/{roleId}", Name = "GetRoleById")]
        public IHttpActionResult GetRoleById(string roleId)
        {
            var result = _roleService.GetRoleById(roleId);
            return Ok(result);
        }

        /**
        * @api {get} /v1/roles Get all roles
        * @apiVersion 1.0.0
        * @apiName GetRoles
        * @apiGroup Role
        *
        * @apiPermission app.permission->list.roles
        *
        * @apiUse AuthHeader
        *
        * @apiSuccess {Object[]} roles List of roles
        * @apiSuccess {String} roles.id Role ID
        * @apiSuccess {String} roles.name Name of the role
        * @apiSuccess {Object[]} roles.permissions Permissions associated to the role
        * @apiSuccess {String} roles.permissions.permissionType Type of the permission
        * @apiSuccess {String} roles.permissions.permissionValue Value of the permission
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
                {
                    "id": "c1a44325-5ece-4ff4-8a41-6b5729e8e65d",
                    "name": "Administrator",
                    "permissions": [
                      {
                        "permissionType": "app.permission",
                        "permissionValue": "assign.role"
                      },
                      {
                        "permissionType": "app.permission",
                        "permissionValue": "create.permission"
                      }
                    ]
                  }
        *   ]
        *
        * @apiUse UnauthorizedError
        **/
        [PermissionAuthorize(PermissionValue = Constants.AppPermissions.ListRoles)]
        [HttpGet]
        [Route("roles")]
        public IHttpActionResult GetRoles()
        {
            var result = _roleService.GetRoles();
            return Ok(result);
        }

        /**
        * @api {post} /v1/roles Create a role
        * @apiVersion 1.0.0
        * @apiName CreateRole
        * @apiGroup Role
        *
        * @apiPermission app.permission->create.role
        * 
        * @apiParamExample {json} Request-Example:
        *   {
        *     "name": "Billing"
        *     "description": "Billing Department"
        *   }
        *
        * @apiUse AuthHeader
        *
        * @apiHeader (Response Headers) {String} location Location of the newly created resource
        *
        * @apiHeaderExample {json} Location-Example
        *   {
        *       "Location": "http://localhost:52431/api/v1/roles/34d87057-fafa-4e5d-822b-cddb1700b507"
        *   }
        *
        * @apiParam {Object} role Role        
        * @apiParam {String} role.name Name of the role
        * @apiParam {String} role.description Description for this role
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 201 CREATED
        *   {
        *       "id": "34d87057-fafa-4e5d-822b-cddb1700b507",
        *       "name": "Billing",
        *       "description": "Billing Department"
        *       "permissions": []
        *   }
        *
        * @apiUse UnauthorizedError
        *
        * @apiUse BadRequestError
        **/
        [PermissionAuthorize(PermissionValue = Constants.AppPermissions.CreateRole)]
        [HttpPost]
        [Route("roles")]
        public async Task<IHttpActionResult> CreateRole(RoleModel model)
        {
            var result = await _roleService.CreateRoleAsync(model);
            return CreatedAtRoute("GetRoleById", new { roleId = result.Id }, result);
        }

        /**
        * @api {post} /v1/roles/:roleId/permissions Create a role permission
        * @apiVersion 1.0.0
        * @apiName AddRolePermission
        * @apiGroup Role Permission
        *
        * @apiPermission app.permission->create.permission
        *
        * @apiHeader (Response Headers) {String} location Location of the newly created resource
        *
        * @apiHeaderExample {json} Location-Example
        *   {
        *       "Location": "http://localhost:52431/api/v1/roles/6d1d5caf-d29d-4bf2-a581-0a35081a1240/permissions/219"
        *   }
        *
        * @apiUse AuthHeader
        *
        * @apiParam {string} roleId Role ID
        * @apiParam {Object} permission Permission
        * @apiParam {String} permission.permissionType Type of the permission
        * @apiParam {String} permission.permissionValue Value of the permission
        *
        * @apiSuccess {Object} permission Permission
        * @apiSuccess {Integer} permission.id Permission ID
        * @apiSuccess {String} permission.permissionType Type
        * @apiSuccess {String} permission.permissionValue Value
        *
        *  @apiSuccessExample Success-Response:
        *   HTTP/1.1 201 Created
        *   {
        *       "permissionType": "app.permission",
        *       "permissionValue": "list.newPermission",
        *       "id": 219
        *   }
        *
        * @apiUse UnauthorizedError
        *
        * @apiUse BadRequestError
        **/
        [PermissionAuthorize(PermissionValue = Constants.AppPermissions.CreatePermission)]
        [Route("roles/{roleId}/permissions")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPermission([FromUri] string roleId, PermissionModel permission)
        {
            var result = await _roleService.AddPermissionAsync(roleId, permission);
            var actionResult = CreatedAtRoute(
                "GetPermissionById",
                new
                {
                    RoleId = roleId,
                    PermissionId = result.Id
                },
                result);
            return actionResult;
        }

        /**
        * @api {get} /v1/roles/:roleId/permissions/:permissionId Get a permission
        * @apiVersion 1.0.0
        * @apiName GetPermissionById
        * @apiGroup Role Permission
        *
        * @apiPermission app.permission->view.permission
        *
        * @apiUse AuthHeader
        *
        * @apiParam {String} roleId Role ID
        * @apiParam {String} permissionId Permission ID
        *
        * @apiSuccess {Object} permission Permission
        * @apiSuccess {Integer} permission.id Permission ID
        * @apiSuccess {String} permission.permissionType Type
        * @apiSuccess {String} permission.permissionValue Value
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   {
        *       "permissionType": "app.permission",
        *       "permissionValue": "list.newPermission",
        *       "id": 219
        *   }
        * @apiUse UnauthorizedError
        * @apiUse NotFoundError
        **/
        [PermissionAuthorize(PermissionValue = Constants.AppPermissions.ViewPermission)]
        [Route("roles/{roleId}/permissions/{permissionId}", Name = "GetPermissionById")]
        [HttpGet]
        public IHttpActionResult GetPermissionById(string roleId, int permissionId)
        {
            var result = _roleService.GetPermissionById(roleId, permissionId);
            return Ok(result);
        }

        /**
        * @api {delete} /v1/roles/:roleId/permissions/:permissionId Remove a role permission
        * @apiVersion 1.0.0
        * @apiName RemoveRolePermission
        * @apiGroup Role Permission
        *
        * @apiPermission app.permission->delete.role-permission
        *
        * @apiUse AuthHeader
        *
        * @apiParam {String} roleId Role ID
        * @apiParam {Integer} permissionId Permission ID
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *
        * @apiUse UnauthorizedError
        *
        **/
        [PermissionAuthorize(PermissionValue = Constants.AppPermissions.DeleteRolePermission)]
        [HttpDelete]
        [Route("roles/{roleId}/permissions/{permissionId}")]
        public IHttpActionResult RemovePermission(string roleId, int permissionId)
        {
            _roleService.RemovePermission(roleId, permissionId);
            return Ok();
        }

        /**
        * @api {get} /v1/roles/:roleId/permissions Get role permissions
        * @apiVersion 1.0.0
        * @apiName GetRolePermissions
        * @apiGroup Role Permission
        *
        * @apiPermission app.permission->list.role-permissions
        *
        * @apiParam {String} roleId Role ID
        *
        * @apiUse AuthHeader
        *
        * @apiSuccess {Object[]} permissions Permissions associated to the role
        * @apiSuccess {String} permissions.permissionType Type of the permission
        * @apiSuccess {String} permissions.permissionValue Value of the permission
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       {
        *           "permissionType": "app.permission",
        *           "permissionValue": "list.newPermission",
        *           "id": 17
        *       }
        *   ]
        *
        * @apiUse UnauthorizedError
        **/
        [PermissionAuthorize(PermissionValue = Constants.AppPermissions.ListRolePermissions)]
        [HttpGet]
        [Route("roles/{roleId}/permissions")]
        public IHttpActionResult GetPermissions(string roleId)
        {
            var result = _roleService.GetRolePermissionsByRoleId(roleId);
            return Ok(result);
        }

        /**
        * @api {delete} /v1/roles/:roleId Delete a role
        * @apiVersion 1.0.0
        * @apiName RemoveRole
        * @apiGroup Role
        *
        *
        * @apiPermission app.permission->delete.role
        *
        * @apiUse AuthHeader
        *
        * @apiParam {String} roleId Role ID
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 204 NO CONTENT
        *
        * @apiUse UnauthorizedError
        *
        * @apiUse NotFoundError
        **/
        [HttpDelete]
        [PermissionAuthorize(PermissionValue = Constants.AppPermissions.DeleteRole)]
        [Route("roles/{roleId}")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string roleId)
        {
            await _roleService.RemoveRoleAsync(roleId);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }
    }
}
