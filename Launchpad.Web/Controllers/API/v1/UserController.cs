using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.Web.Extensions;
using Launchpad.Web.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.Controllers.API.V1
{
    [Authorize]
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class UserController : ApiController
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));

        }

        /**
        * @api {get} /v1/user Get all users
        * @apiVersion 0.1.0
        * @apiName GetUsers
        * @apiGroup User
        * 
        * @apiPermission lss.permission->list.users
        * 
        * @apiUse AuthHeader
        *   
        * @apiSuccess {Object[]} users List of users 
        * @apiSuccess {String} users.id User ID
        * @apiSuccess {String} users.name Name of the user
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       {   
        *           "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *           "name": "admin@admin.com"
        *       }
        *   ]
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.ListUsers)]
        [HttpGet]
        [Route("user")]
        public IHttpActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }


        /**
        * @api {get} /v1/user/roles Get all users and their roles 
        * @apiVersion 0.1.0
        * @apiName User
        * @apiGroup User
        * 
        * @apiPermission lss.permission->list.users
        * 
        * @apiUse AuthHeader
        *   
        * @apiSuccess {Object[]} users List of users 
        * @apiSuccess {String} users.id User ID
        * @apiSuccess {String} users.name Name of the user
        * @apiSuccess {Object[]} users.roles List of user roles
        * @apiSuccess users.roles.name Role name
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       {   
        *           "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *           "name": "admin@admin.com",
        *           "roles": [ 
        *                    {
        *                        "name": "Admin"
        *                    }
        *            ]
        *       }
        *   ]
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.ListUsers)]        
        [Route("user/roles")]
        [HttpGet]
        public IHttpActionResult GetUsersWithRoles()
        {
            return Ok(_userService.GetUsersWithRoles());
        }


        /**
        * @api {get} /v1/user/:userId/claims Get user claims
        * @apiVersion 0.1.0
        * @apiName Claims
        * @apiGroup User
        * 
        * @apiPermission lss.permission->list.user-claims
        * 
        * @apiUse AuthHeader
        * 
        * @apiParam {String} userId Id of user
        *   
        * @apiSuccess {Object[]} claims List of user claims
        * @apiSuccess {String} claims.claimType Type of the claim
        * @apiSuccess {String} claims.claimValue Value of the claim
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       {   
        *           "claimType": "lss.permission",
        *           "claimValue": "login"
        *       },
        *       {
        *           "claimType": "lss.permission",
        *           "claimValue": "list.user-claims"
        *       }
        *   ]
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.ListUserClaims)]
        [HttpGet]
        [Route("user/{userId}/claims")]
        public async Task<IHttpActionResult> GetClaims(string userId)
        {
            var claims = await _userService.GetUserClaimsAsync(userId);
            return Ok(claims);
        }


        /**
        * @api {post} /v1/user/:userId/role Assign role to user 
        * @apiVersion 0.1.0
        * @apiName AssignRole
        * @apiGroup User
        * 
        * @apiPermission lss.permission->assign.role
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {String} userId User ID
        * @apiParam {Object} role Role for the association
        * @apiParam {String} role.id Role ID
        * @apiParam {String} role.name Name of the role
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.AssignRole)]
        [HttpPost]
        [Route("user/{userId}/role")]
        public async Task<IHttpActionResult> AssignRole([FromUri] string userId, RoleModel roleModel)
        {
            var identityResult = await _userService.AssignUserRoleAsync(userId, roleModel);

            if (!identityResult.Succeeded)
            {
                ModelState.AddErrors(identityResult);
                return BadRequest(ModelState);
            }

            return Ok();
        }


        /**
        * @api {delete} /v1/user/:userId/role/:roleId Remove role from user 
        * @apiVersion 0.1.0
        * @apiName RevokeRole
        * @apiGroup User
        * 
        * @apiPermission lss.permission->revoke.role
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {String} roleId Role ID
        * @apiParam {String} userId User ID
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue =LssClaims.RevokeRole)]
        [HttpDelete]
        [Route("user/{userId}/role/{roleId}")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string userId, [FromUri] string roleId)
        {
            var result = await _userService.RemoveUserFromRoleAsync(userId, roleId);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddErrors(result);
                if (ModelState.IsValid)
                    return BadRequest();
                else
                    return BadRequest(ModelState);
            }
        }
    }
}
