using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.Web.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API.V1
{
    [Authorize]
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService.ThrowIfNull(nameof(roleService));
        }

        /**
        * @api {get} /v1/roles/:roleId Get a role
        * @apiVersion 0.1.0
        * @apiName GetRoleById
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->view.role
        * 
        * @apiUse AuthHeader
        * 
        * @apiParam {String} roleId Role ID
        *  
        * @apiSuccess {Object} role  
        * @apiSuccess {String} role.id Role ID
        * @apiSuccess {String} role.name Name of the role
        * @apiSuccess {Object[]} role.claims Claims associated to the role
        * @apiSuccess {String} role.claims.claimType Type of the claim
        * @apiSuccess {String} role.claims.claimValue Value of the claim
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *      {
        *           "id": "76d216ab-cb48-4c5f-a4ba-1e9c3bae1fe6",
        *           "name": "New Role 1",
        *           "claims": [
        *               {
        *                   claimType: "lss.permission",
        *                   claimValue: "view.role"
        *               }
        *           ]
        *       }
        *   ]
        *   
        * @apiUse UnauthorizedError  
        * @apiUse NotFoundError
        **/
        [ClaimAuthorize(ClaimValue = Constants.LssClaims.ViewRole)]
        [HttpGet]
        [Route("roles/{roleId}", Name = "GetRoleById")]
        public IHttpActionResult GetRoleById(string roleId)
        {
            var entityResult = _roleService.GetRoleById(roleId);
            return CreateModelResult(entityResult);
        }

        /**
        * @api {get} /v1/roles Get all roles
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
        [ClaimAuthorize(ClaimValue = Constants.LssClaims.ListRoles)]
        [HttpGet]
        [Route("roles")]
        public IHttpActionResult GetRoles()
        {
            var entityResult = _roleService.GetRoles();
            return CreateModelResult(entityResult);
        }

        /**
        * @api {post} /v1/roles Create a role 
        * @apiVersion 0.1.0
        * @apiName CreateRole
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->create.role
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
        * @apiParam {String} role.id Role ID
        * @apiParam {String} role.name Name of the role
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 201 CREATED
        *   {
        *       "id": "34d87057-fafa-4e5d-822b-cddb1700b507",
        *       "name": "New Role 2",
        *       "claims": []
        *   }
        *   
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue = Constants.LssClaims.CreateRole)]
        [HttpPost]
        [Route("roles")]
        public async Task<IHttpActionResult> CreateRole(RoleModel model)
        {
            var entityResult = await _roleService.CreateRoleAsync(model);
            return CreatedEntityAt("GetRoleById", () => new { roleId = entityResult.Model.Id }, entityResult);
        }

        /**
        * @api {post} /v1/roles/:roleId/claims Create a role claim 
        * @apiVersion 0.1.0
        * @apiName AddRoleClaim
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->create.claim
        * 
        * @apiHeader (Response Headers) {String} location Location of the newly created resource
        *   
        * @apiHeaderExample {json} Location-Example
        *   { 
        *       "Location": "http://localhost:52431/api/v1/roles/6d1d5caf-d29d-4bf2-a581-0a35081a1240/claims/219"
        *   }
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {string} roleId Role ID 
        * @apiParam {Object} claim Claim 
        * @apiParam {String} claim.claimType Type of the claim 
        * @apiParam {String} claim.claimValue Value of the claim
        * 
        * @apiSuccess {Object} claim Claim
        * @apiSuccess {Integer} claim.id Claim ID
        * @apiSuccess {String} claim.claimType Type
        * @apiSuccess {String} claim.claimValue Value
        * 
        *  @apiSuccessExample Success-Response:
        *   HTTP/1.1 201 Created
        *   {
        *       "claimType": "lss.permission",
        *       "claimValue": "list.widgets",
        *       "id": 219
        *   }
        *
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue = Constants.LssClaims.CreateClaim)]
        [Route("roles/{roleId}/claims")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClaim([FromUri] string roleId, ClaimModel claim)
        {
            var entityResult = await _roleService.AddClaimAsync(roleId, claim);
            var actionResult = CreatedEntityAt(
                "GetClaimById",
                () => new
                {
                    RoleId = roleId,
                    ClaimId = entityResult.Model.Id
                },
                entityResult);
            return actionResult;
        }

        /**
        * @api {get} /v1/roles/:roleId/claims/:claimId Get a claim
        * @apiVersion 0.1.0
        * @apiName GetClaimById
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->view.claim
        *
        * @apiUse AuthHeader
        *   
        * @apiParam {String} roleId Role ID 
        * @apiParam {String} claimId Claim ID
        * 
        * @apiSuccess {Object} claim Claim
        * @apiSuccess {Integer} claim.id Claim ID
        * @apiSuccess {String} claim.claimType Type
        * @apiSuccess {String} claim.claimValue Value
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 Created
        *   {
        *       "claimType": "lss.permission",
        *       "claimValue": "list.widgets",
        *       "id": 219
        *   }
        * @apiUse UnauthorizedError
        * @apiUse NotFoundError
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue = Constants.LssClaims.ViewClaim)]
        [Route("roles/{roleId}/claims/{claimId}", Name = "GetClaimById")]
        [HttpGet]
        public IHttpActionResult GetClaimById(string roleId, int claimId)
        {
            var entityResult = _roleService.GetClaimById(roleId, claimId);
            return CreateModelResult(entityResult);
        }

        /**
        * @api {delete} /v1/roles/:roleId/claims/:claimId Remove a role claim 
        * @apiVersion 0.1.0
        * @apiName RemoveRoleClaim
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->delete.role-claim
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {String} roleId Role ID
        * @apiParam {Integer} claimId Claim ID
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 204 No Content
        *
        * @apiUse UnauthorizedError
        * 
        **/
        [ClaimAuthorize(ClaimValue = Constants.LssClaims.DeleteRoleClaim)]
        [HttpDelete]
        [Route("roles/{roleId}/claims/{claimId}")]
        public IHttpActionResult RemoveClaim(string roleId, int claimId)
        {
            var entityResult = _roleService.RemoveClaim(roleId, claimId);

            return NoContent(entityResult);
        }

        /**
        * @api {get} /v1/roles/:roleId/claims Get role claims
        * @apiVersion 0.1.0
        * @apiName GetRoleClaims
        * @apiGroup Role
        * 
        * @apiPermission lss.permission->list.role-claims
        * 
        * @apiParam {String} roleId Role ID
        * 
        * @apiUse AuthHeader
        *   
        * @apiSuccess {Object[]} claims Claims associated to the role
        * @apiSuccess {String} claims.claimType Type of the claim
        * @apiSuccess {String} claims.claimValue Value of the claim
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       { 
        *           "claimType": "lss.permission",
        *           "claimValue": "list.widgets",
        *           "id": 17
        *       }
        *   ]
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = Constants.LssClaims.ListRoleClaims)]
        [HttpGet]
        [Route("roles/{roleId}/claims")]
        public IHttpActionResult GetClaims(string roleId)
        {
            var entityResult = _roleService.GetRoleClaimsByRoleId(roleId);
            return CreateModelResult(entityResult);
        }

        /**
        * @api {delete} /v1/roles/:roleId Delete a role 
        * @apiVersion 0.1.0
        * @apiName RemoveRole
        * @apiGroup Role
        * 
        * 
        * @apiPermission lss.permission->delete.role
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {String} roleId Role ID
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 204 No Content
        *
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [HttpDelete]
        [ClaimAuthorize(ClaimValue = Constants.LssClaims.DeleteRole)]
        [Route("roles/{roleId}")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string roleId)
        {
            var result = await _roleService.RemoveRoleAsync(roleId);
            return NoContent(result);
        }
    }
}
