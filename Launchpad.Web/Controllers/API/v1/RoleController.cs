using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.Web.Filters;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.Controllers.API.V1
{
    [Authorize]
    [RoutePrefix(RoutePrefixes.V1)]
    public class RoleController : ApiController
    {
        private IRoleService _roleService;


        public RoleController(IRoleService roleService)
        {
            _roleService = roleService.ThrowIfNull(nameof(roleService));
        }


        /**
        * @api {get} /v1/role Get all roles
        * @apiVersion 0.1.0
        * @apiName GetRoles
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->list.roles
        * 
        * @apiUse AuthHeader
        *   
        * @apiSuccess {Object[]} roles List of roles 
        * @apiSuccess {String} roles.id Role ID
        * @apiSuccess {String} roles.name Name of the role
        * @apiSuccess {Object[]} roles.claims Claims associated to the role
        * @apiSuccess {String} roles.claims.claimType Type of the claim
        * @apiSuccess {String} roles.claims.claimValue Value of the claim
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
                {
                    "id": "c1a44325-5ece-4ff4-8a41-6b5729e8e65d",
                    "name": "Administrator",
                    "claims": [
                      {
                        "claimType": "lss.permission",
                        "claimValue": "assign.role"
                      },
                      {
                        "claimType": "lss.permission",
                        "claimValue": "create.claim"
                      }
                    ]
                  }
        *   ]
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.ListRoles)]
        [HttpGet]
        [Route("role")]
        public IHttpActionResult GetRoles()
        {
            var result = _roleService.GetRoles();
            return Ok(result);
        }

        /**
        * @api {post} /v1/role Create a role 
        * @apiVersion 0.1.0
        * @apiName CreateRole
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->create.role
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {Object} role Role
        * @apiParam {String} role.id Role ID
        * @apiParam {String} role.name Name of the role
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 201 CREATED
        *
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.CreateRole)]
        [HttpPost]
        [Route("role")]
        public async Task<IHttpActionResult> CreateRole(RoleModel model)
        {
            var result = await _roleService.CreateRoleAsync(model);
            if (result.Succeeded)
            {
                return StatusCode(HttpStatusCode.Created);
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        /**
       * @api {post} /v1/role/claim Create a role claim 
       * @apiVersion 0.1.0
       * @apiName AddRoleClaim
       * @apiGroup Role
       * 
       * @apiPermission lss.permission->create.claim
       * 
       * @apiUse AuthHeader
       *   
       * @apiParam {Object} roleClaim Association between a role and a claim 
       * @apiParam {Object} roleClaim.role Role
       * @apiParam {String} roleClaim.role.id Role ID
       * @apiParam {String} roleClaim.role.name Name of the role
       * @apiParam {Object} roleClaim.claim Claim 
       * @apiParam {String} roleClaim.claim.claimType Type of the claim 
       * @apiParam {String} roleClaim.claim.claimValue Value of the claim
       * 
       * @apiSuccessExample Success-Response:
       *   HTTP/1.1 201 Created
       *
       * @apiUse UnauthorizedError
       * 
       * @apiUse BadRequestError  
       **/
        [ClaimAuthorize(ClaimValue = LssClaims.CreateClaim)]
        [Route("role/claim")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClaim(RoleClaimModel model)
        {
            await _roleService.AddClaimAsync(model.Role, model.Claim);
            return StatusCode(HttpStatusCode.Created);
        }



        /**
        * @api {delete} /v1/role/claim Remove a role claim 
        * @apiVersion 0.1.0
        * @apiName RemoveRoleClaim
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->delete.role-claim
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {String} roleName Name of the role
        * @apiParam {String} claimType Type of the claim 
        * @apiParam {String} claimValue Value of the claim
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 204 No Content
        *
        * @apiUse UnauthorizedError
        * 
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.DeleteRoleClaim)]
        [HttpDelete]
        [Route("role/claim")]
        public IHttpActionResult RemoveClaim(string roleName, string claimType, string claimValue)
        {
            _roleService.RemoveClaim(roleName, claimType, claimValue);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /**
        * @api {delete} /v1/role Delete a role 
        * @apiVersion 0.1.0
        * @apiName RemoveRole
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->delete.role
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {Object} role Role
        * @apiParam {String} role.id Role ID
        * @apiParam {String} role.name Name of the role
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 204 No Content
        *
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [HttpDelete]
        [ClaimAuthorize(ClaimValue = LssClaims.DeleteRole)]
        [Route("role")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] RoleModel role)
        {
            var result = await _roleService.RemoveRoleAsync(role);
            if (result.Succeeded)
                return StatusCode(HttpStatusCode.NoContent);
            else
                return BadRequest();
        }



    }
}
