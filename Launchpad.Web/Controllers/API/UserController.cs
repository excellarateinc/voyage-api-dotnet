using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Launchpad.Web.Extensions;
using Launchpad.Web.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using static Launchpad.Web.Constants;

namespace Launchpad.Web.Controllers.API
{
    [Authorize]
    [RoutePrefix(Constants.RoutePrefixes.User)]
    public class UserController : ApiController
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));

        }

        /**
        * @api {get} /user Get all users
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
        public IHttpActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }


        /**
        * @api {get} /user/roles Get all users and their roles 
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
        * @api {get} /user/claims Get the authenticated user's claims 
        * @apiVersion 0.1.0
        * @apiName Claims
        * @apiGroup User
        * 
        * @apiPermission lss.permission->list.user-claims
        * 
        * @apiUse AuthHeader
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
        [ClaimAuthorize(ClaimValue =LssClaims.ListUserClaims)]
        [HttpGet]
        [Route("user/claims")]
        public IHttpActionResult GetClaims()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var clientFacingClaims = claimsIdentity.Claims
                .Where(_=>_.Type == Constants.LssClaims.Type)
                .Select(_ => new ClaimModel { ClaimType = _.Type, ClaimValue = _.Value })
                .ToList();
            return Ok(clientFacingClaims);
        }


         /**
         * @api {post} /user/assign Assign role to user 
         * @apiVersion 0.1.0
         * @apiName AssignRole
         * @apiGroup User
         * 
         * @apiPermission lss.permission->assign.role
         * 
         * @apiUse AuthHeader
         *   
         * @apiParam {Object} userRole Association between a user and a role
         * @apiParam {Object} userRole.role Role for the association
         * @apiParam {String} userRole.role.id Role ID
         * @apiParam {String} userRole.role.name Name of the role
         * @apiParam {Object} userRole.user User for the association
         * @apiParam {String} userRole.user.id User ID
         * @apiParam {String} userRole.user.name Name of the user
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
        [Route("user/assign")]
        public async Task<IHttpActionResult> AssignRole(UserRoleModel userRole)
        {
            var identityResult = await _userService.AssignUserRoleAsync(userRole.Role, userRole.User);

            if (!identityResult.Succeeded)
            {
                return StatusCode(System.Net.HttpStatusCode.BadRequest);
            }

            return Ok();
        }


        /**
        * @api {post} /user/revoke Remove role from user 
        * @apiVersion 0.1.0
        * @apiName RevokeRole
        * @apiGroup User
        * 
        * @apiPermission lss.permission->revoke.role
        * 
        * @apiUse AuthHeader
        *   
        * @apiParam {Object} userRole Association between a user and a role
        * @apiParam {Object} userRole.role Role for the association
        * @apiParam {String} userRole.role.id Role ID
        * @apiParam {String} userRole.role.name Name of the role
        * @apiParam {Object} userRole.user User for the association
        * @apiParam {String} userRole.user.id User ID
        * @apiParam {String} userRole.user.name Name of the user
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue =LssClaims.RevokeRole)]
        [HttpPost]
        [Route("user/revoke")]
        public async Task<IHttpActionResult> RemoveRole(UserRoleModel model)
        {
            var result = await _userService.RemoveUserFromRoleAsync(model.Role, model.User);
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
