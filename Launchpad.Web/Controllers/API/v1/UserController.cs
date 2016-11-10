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
        * @api {get} /v1/users Get all users
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
        * @apiSuccess {String} users.firstName First Name
        * @apiSuccess {String} users.lastName last name
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       {   
        *           "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *           "name": "admin@admin.com",
        *           "firstName": "Admin_First",
        *           "lastName": "Admin_Last"
        *       }
        *   ]
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.ListUsers)]
        [HttpGet]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        /**
        * @api {put} /v1/users/:userId Update user
        * @apiVersion 0.1.0
        * @apiName UpdateUser
        * @apiGroup User
        * 
        * @apiPermission lss.permission->update.user
        * 
        * @apiUse AuthHeader
        * 
        * @apiParam {String} userId User ID
        * @apiParam {Object} user User
        * @apiParam {String} user.name Name of the user
        * @apiParam {String} user.id User ID  
        * @apiParam {String} user.firstName First name
        * @apiParam {String} user.lastName Last name  
        *   
        * @apiSuccess {Object} user User 
        * @apiSuccess {String} user.id User ID
        * @apiSuccess {String} user.name Name of the user
        * @apiSuccess {String} user.firstName First name
        * @apiSuccess {String} user.lastName Last name
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   {   
        *           "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *           "name": "admin@admin.com",
        *           "firstName": "Admin_First",
        *           "lastName": "Admin_Last"
        *           
        *   }
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.UpdateUser)]
        [HttpPut]
        [Route("users/{userId}")]
        public async Task<IHttpActionResult> UpdateUser([FromUri] string userId, [FromBody] UserModel userModel)
        {
            var result = await _userService.UpdateUser(userId, userModel);
            return Ok(result.Model);
        }

        /**
        * @api {delete} /v1/users/:userId Delete a user
        * @apiVersion 0.1.0
        * @apiName DeleteUser
        * @apiGroup User
        * 
        * @apiPermission lss.permission->delete.user
        * 
        * @apiUse AuthHeader
        *  
        * @apiParam {String} userId User ID  
        *   
        *            
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 204 NO CONTENT  
        *   
        *   
        * @apiUse UnauthorizedError
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.DeleteUser)]
        [HttpDelete]
        [Route("users/{userId}")]
        public async Task<IHttpActionResult> DeleteUser([FromUri] string userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (result.Succeeded)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }else
            {
                ModelState.AddErrors(result);
                return BadRequest(ModelState);
            }
        }

        /**
        * @api {post} /v1/users Create user
        * @apiVersion 0.1.0
        * @apiName CreateUser
        * @apiGroup User
        * 
        * @apiPermission lss.permission->create.user
        * 
        * @apiUse AuthHeader
        *
        * @apiHeader (Response Headers) {String} location Location of the newly created resource
        *   
        * @apiHeaderExample {json} Location-Example
        *   { 
        *       "Location": "http://localhost:52431/api/v1/users/b78ae241-1fa6-498c-aa48-9742245d0d2f"
        *   }
        * 
        * 
        * @apiParam {Object} user User
        * @apiParam {String} user.name Name of the user
        * @apiParam {String} user.firstName First name
        * @apiParam {String} user.lastName Last name
        *   
        * @apiSuccess {Object} user User 
        * @apiSuccess {String} users.id User ID
        * @apiSuccess {String} users.name Name of the user
        * @apiSuccess {String} user.firstName First name
        * @apiSuccess {String} user.lastName Last name
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 201 CREATED
        *   {   
        *           "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *           "name": "admin@admin.com"
        *           "firstName": "Admin_First",
        *           "lastName": "Admin_Last"
        *   }
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.CreateUser)]
        [HttpPost]
        [Route("users")]
        public async Task<IHttpActionResult> CreateUser(UserModel user)
        {
            var result = await _userService.CreateUserAsync(user);
            if (result.Result.Succeeded)
            {
                return CreatedAtRoute("GetUser", new { UserId = result.Model.Id }, result.Model);
            }else
            {
                ModelState.AddErrors(result.Result);
                return BadRequest(ModelState);
            }
        }

        /**
        * @api {get} /v1/users/:userId Get user
        * @apiVersion 0.1.0
        * @apiName GetUser
        * @apiGroup User
        * 
        * @apiPermission lss.permission->view.user
        * 
        * @apiUse AuthHeader
        *  
        * @apiParam {String} userId User ID  
        *   
        * @apiSuccess {Object} user User
        * @apiSuccess {String} user.id User ID
        * @apiSuccess {String} user.name Name of the user
        * @apiSuccess {String} user.firstName First name
        * @apiSuccess {String} user.lastName Last name
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK  
        *   {   
        *       "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *       "name": "admin@admin.com",
        *       "firstName": "Admin_first",
        *       "lastName": "Admin_last"
        *       
        *   }
        *   
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.ViewUser)]
        [HttpGet]
        [Route("users/{userId}", Name ="GetUser")]
        public async Task<IHttpActionResult> GetUser(string userId)
        {
            var user = await _userService.GetUser(userId);
            return Ok(user);
        }

        /**
        * @api {get} /v1/users/:userId/roles Get user roles 
        * @apiVersion 0.1.0
        * @apiName User
        * @apiGroup User
        * 
        * @apiPermission lss.permission->list.users
        * 
        * @apiUse AuthHeader
        *
        * @apiParam {String} userId ID of the user 
        *   
        * @apiSuccess {Object[]} role List of roles 
        * @apiSuccess {String} role.id Role ID
        * @apiSuccess {String} role.name Name of the role     
        * @apiSuccess {Object[]} claims List of claims
        * @apiSuccess {String} claims.claimType Type of claim
        * @apiSuccess {String} claims.claimValue Value of claim 
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *       {
        *           "id": "7ec91144-a60e-4240-8878-ccba3c4c2ef4",
        *           "name": "Basic",
        *           "claims": [
        *               {
        *                   "claimType": "lss.permission",
        *                   "claimValue": "login"
        *               },
        *               {
        *                   "claimType": "lss.permission",
        *                   "claimValue": "list.user-claims"
        *               }
        *   ]
        *
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.ListUsers)]        
        [Route("users/{userId}/roles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserRoles(string userId)
        {
            var roles = await _userService.GetUserRolesAsync(userId);
            return Ok(roles);
        }


        /**
        * @api {get} /v1/users/:userId/claims Get user claims
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
        [Route("users/{userId}/claims")]
        public async Task<IHttpActionResult> GetClaims(string userId)
        {
            var claims = await _userService.GetUserClaimsAsync(userId);
            return Ok(claims);
        }


        /**
        * @api {post} /v1/users/:userId/roles Assign role to user 
        * @apiVersion 0.1.0
        * @apiName AssignRole
        * @apiGroup User
        * 
        * @apiPermission lss.permission->assign.role
        * 
        * @apiUse AuthHeader
        *   
        * @apiHeader (Response Headers) {String} location Location of the newly created resource
        *   
        * @apiHeaderExample {json} Location-Example
        *   { 
        *       "Location": "http://localhost:52431/api/v1/users/ceee08c8-9b3b-4fde-a234-86cc04993309/roles/76d216ab-cb48-4c5f-a4ba-1e9c3bae1fe6"
        *   }
        *   
        * @apiParam {String} userId User ID
        * @apiParam {Object} role Role for the association
        * @apiParam {String} role.id Role ID
        * @apiParam {String} role.name Name of the role
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 201 CREATED
        *   {
        *       "id": "76d216ab-cb48-4c5f-a4ba-1e9c3bae1fe6",
        *       "name": "New Role 1",
        *       "claims": []
        *   }
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue = LssClaims.AssignRole)]
        [HttpPost]
        [Route("users/{userId}/roles")]
        public async Task<IHttpActionResult> AssignRole([FromUri] string userId, RoleModel roleModel)
        {
            var identityResult = await _userService.AssignUserRoleAsync(userId, roleModel);

            if (!identityResult.Result.Succeeded)
            {
                ModelState.AddErrors(identityResult.Result);
                return BadRequest(ModelState);
            }

            return CreatedAtRoute("GetUserRoleById", new { UserId = userId, RoleId = identityResult.Model.Id }, identityResult.Model);
        }


        /**
        * @api {get} /v1/users/:userId/roles/:roleId Get role 
        * @apiVersion 0.1.0
        * @apiName GetUserRoleById
        * @apiGroup User
        * 
        * @apiPermission lss.permission->view.role
        * 
        * @apiUse AuthHeader
        *
        * @apiParam {String} userId User ID 
        * @apiParam {String} roleId Role ID  
        *   
        * @apiSuccess {Object} role Role 
        * @apiSuccess {String} role.id Role ID
        * @apiSuccess {String} role.name Name of the role     
        * @apiSuccess {Object[]} role.claims List of claims
        * @apiSuccess {String} role.claims.claimType Type of claim
        * @apiSuccess {String} role.claims.claimValue Value of claim 
        * 
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *       {
        *           "id": "7ec91144-a60e-4240-8878-ccba3c4c2ef4",
        *           "name": "Basic",
        *           "claims": [
        *               {
        *                   "claimType": "lss.permission",
        *                   "claimValue": "login"
        *               },
        *               {
        *                   "claimType": "lss.permission",
        *                   "claimValue": "list.user-claims"
        *               }
        *       }
        *
        *   
        * @apiUse UnauthorizedError  
        **/
        [ClaimAuthorize(ClaimValue =LssClaims.ViewRole)]
        [HttpGet]
        [Route("users/{userId}/roles/{roleId}", Name = "GetUserRoleById")]
        public IHttpActionResult GetUserRoleById(string userId, string roleId)
        {
            return Ok(_userService.GetUserRoleById(userId, roleId));
        }

        /**
        * @api {delete} /v1/users/:userId/roles/:roleId Remove role from user 
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
        *   HTTP/1.1 204 No Content
        *
        * @apiUse UnauthorizedError
        * 
        * @apiUse BadRequestError  
        **/
        [ClaimAuthorize(ClaimValue =LssClaims.RevokeRole)]
        [HttpDelete]
        [Route("users/{userId}/roles/{roleId}")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string userId, [FromUri] string roleId)
        {
            var result = await _userService.RemoveUserFromRoleAsync(userId, roleId);
            if (result.Succeeded)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
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
