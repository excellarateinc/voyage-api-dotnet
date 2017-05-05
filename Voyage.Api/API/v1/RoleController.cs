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
        * @apiVersion 0.1.0
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
        *                   claimType: "app.permission",
        *                   claimValue: "view.role"
        *               }
        *           ]
        *       }
        *   ]
        *
        * @apiUse UnauthorizedError
        * @apiUse NotFoundError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ViewRole)]
        [HttpGet]
        [Route("roles/{roleId}", Name = "GetRoleById")]
        public IHttpActionResult GetRoleById(string roleId)
        {
            var result = _roleService.GetRoleById(roleId);
            return Ok(result);
        }

        /**
        * @api {get} /v1/roles Get all roles
        * @apiVersion 0.1.0
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
                        "claimType": "app.permission",
                        "claimValue": "assign.role"
                      },
                      {
                        "claimType": "app.permission",
                        "claimValue": "create.claim"
                      }
                    ]
                  }
        *   ]
        *
        * @apiUse UnauthorizedError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ListRoles)]
        [HttpGet]
        [Route("roles")]
        public IHttpActionResult GetRoles()
        {
            var result = _roleService.GetRoles();
            return Ok(result);
        }

        /**
        * @api {post} /v1/roles Create a role
        * @apiVersion 0.1.0
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
        *       "claims": []
        *   }
        *
        * @apiUse UnauthorizedError
        *
        * @apiUse BadRequestError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.CreateRole)]
        [HttpPost]
        [Route("roles")]
        public async Task<IHttpActionResult> CreateRole(RoleModel model)
        {
            var result = await _roleService.CreateRoleAsync(model);
            return CreatedAtRoute("GetRoleById", new { roleId = result.Id }, result);
        }

        /**
        * @api {post} /v1/roles/:roleId/claims Create a role claim
        * @apiVersion 0.1.0
        * @apiName AddRoleClaim
        * @apiGroup Role Claim
        *
        * @apiPermission app.permission->create.claim
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
        *       "claimType": "app.permission",
        *       "claimValue": "list.newClaim",
        *       "id": 219
        *   }
        *
        * @apiUse UnauthorizedError
        *
        * @apiUse BadRequestError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.CreateClaim)]
        [Route("roles/{roleId}/claims")]
        [HttpPost]
        public async Task<IHttpActionResult> AddClaim([FromUri] string roleId, ClaimModel claim)
        {
            var result = await _roleService.AddClaimAsync(roleId, claim);
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

        /**
        * @api {get} /v1/roles/:roleId/claims/:claimId Get a claim
        * @apiVersion 0.1.0
        * @apiName GetClaimById
        * @apiGroup Role Claim
        *
        * @apiPermission app.permission->view.claim
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
        *   HTTP/1.1 200 OK
        *   {
        *       "claimType": "app.permission",
        *       "claimValue": "list.newClaim",
        *       "id": 219
        *   }
        * @apiUse UnauthorizedError
        * @apiUse NotFoundError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ViewClaim)]
        [Route("roles/{roleId}/claims/{claimId}", Name = "GetClaimById")]
        [HttpGet]
        public IHttpActionResult GetClaimById(string roleId, int claimId)
        {
            var result = _roleService.GetClaimById(roleId, claimId);
            return Ok(result);
        }

        /**
        * @api {delete} /v1/roles/:roleId/claims/:claimId Remove a role claim
        * @apiVersion 0.1.0
        * @apiName RemoveRoleClaim
        * @apiGroup Role Claim
        *
        * @apiPermission app.permission->delete.role-claim
        *
        * @apiUse AuthHeader
        *
        * @apiParam {String} roleId Role ID
        * @apiParam {Integer} claimId Claim ID
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *
        * @apiUse UnauthorizedError
        *
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.DeleteRoleClaim)]
        [HttpDelete]
        [Route("roles/{roleId}/claims/{claimId}")]
        public IHttpActionResult RemoveClaim(string roleId, int claimId)
        {
            _roleService.RemoveClaim(roleId, claimId);
            return Ok();
        }

        /**
        * @api {get} /v1/roles/:roleId/claims Get role claims
        * @apiVersion 0.1.0
        * @apiName GetRoleClaims
        * @apiGroup Role Claim
        *
        * @apiPermission app.permission->list.role-claims
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
        *           "claimType": "app.permission",
        *           "claimValue": "list.newClaim",
        *           "id": 17
        *       }
        *   ]
        *
        * @apiUse UnauthorizedError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ListRoleClaims)]
        [HttpGet]
        [Route("roles/{roleId}/claims")]
        public IHttpActionResult GetClaims(string roleId)
        {
            var result = _roleService.GetRoleClaimsByRoleId(roleId);
            return Ok(result);
        }

        /**
        * @api {delete} /v1/roles/:roleId Delete a role
        * @apiVersion 0.1.0
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
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.DeleteRole)]
        [Route("roles/{roleId}")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string roleId)
        {
            await _roleService.RemoveRoleAsync(roleId);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }
    }
}
