using System.Net;
using System.Net.Http;
using Voyage.Core;
using Voyage.Models;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Api.Filters;
using Voyage.Services.User;

namespace Voyage.Api.API.V1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

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
        * @apiPermission app.permission->list.users
        *
        * @apiUse AuthHeader
        *
        * @apiSuccess {Object[]} users List of users
        * @apiSuccess {String} users.id User ID
        * @apiSuccess {String} users.userName Username of the user
        * @apiSuccess {String} users.email Email
        * @apiSuccess {String} users.firstName First name
        * @apiSuccess {String} users.lastName Last name
        * @apiSuccess {Object[]} users.phones User phone numbers
        * @apiSuccess {String} users.phones.phoneNumber Phone number
        * @apiSuccess {String} users.phones.phoneType Phone type
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *      {
        *          "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *          "userName": "admin",
        *          "email": "admin@admin.com",
        *          "firstName": "Admin_First",
        *          "lastName": "Admin_Last",
        *          "isActive": true,
        *          "isVerifyRequired": true,
        *          "phones":
        *          [
        *             {
        *                 "phoneNumber": "123-123-1233",
        *                 "phoneType": "mobile"
        *             }
        *          ]
        *       }
        *   ]
        *
        * @apiUse UnauthorizedError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ListUsers)]
        [HttpGet]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        /**
        * @api {put} /v1/users/:userId Update user
        * @apiVersion 0.1.0
        * @apiName UpdateUserAsync
        * @apiGroup User
        *
        * @apiPermission app.permission->update.user
        *
        * @apiUse AuthHeader
        *
        * @apiUse UserRequestModel
        * @apiUse UserSuccessModel
        * @apiUse UnauthorizedError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.UpdateUser)]
        [HttpPut]
        [Route("users/{userId}")]
        public async Task<IHttpActionResult> UpdateUser([FromUri] string userId, [FromBody] UserModel userModel)
        {
            var result = await _userService.UpdateUserAsync(userId, userModel);
            return Ok(result);
        }

        /**
        * @api {delete} /v1/users/:userId Delete a user
        * @apiVersion 0.1.0
        * @apiName DeleteUserAsync
        * @apiGroup User
        *
        * @apiPermission app.permission->delete.user
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
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.DeleteUser)]
        [HttpDelete]
        [Route("users/{userId}")]
        public async Task<IHttpActionResult> DeleteUser([FromUri] string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result.Succeeded)
                return BadRequest();

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        /**
        * @api {post} /v1/users Create user
        * @apiVersion 0.1.0
        * @apiName CreateUser
        * @apiGroup User
        *
        * @apiPermission app.permission->create.user
        * 
        * @apiParamExample {json} Request-Example:
        *      {
        *          "userName": "John",
        *          "email": "John@John.com",
        *          "firstName": "John FirstName",
        *          "lastName": "John LastName",
        *          "phones":
        *          [
        *             {
        *                 "phoneNumber": "123-123-1233",
        *                 "phoneType": "mobile"
        *             }
        *          ]
        *       }
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
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *      {
        *          "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *          "userName": "John",
        *          "email": "John@John.com",
        *          "firstName": "John FirstName",
        *          "lastName": "John LastName",
        *          "isActive": true,
        *          "isVerifyRequired": true,
        *          "phones":
        *          [
        *             {
        *                 "phoneNumber": "123-123-1233",
        *                 "phoneType": "mobile"
        *             }
        *          ]
        *       }
        *   ]
        * @apiUse UnauthorizedError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.CreateUser)]
        [HttpPost]
        [Route("users")]
        public async Task<IHttpActionResult> CreateUser(UserModel user)
        {
            var result = await _userService.CreateUserAsync(user);
            return CreatedAtRoute("GetUserAsync", new { UserId = result.Id }, result);
        }

        /**
        * @api {get} /v1/users/:userId Get user
        * @apiVersion 0.1.0
        * @apiName GetUserAsync
        * @apiGroup User
        *
        * @apiPermission app.permission->view.user
        *
        * @apiUse AuthHeader
        *
        * @apiParam {String} userId User ID
        *
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 200 OK
        *   [
        *      {
        *          "id": "A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A",
        *          "userName": "John",
        *          "email": "John@John.com",
        *          "firstName": "John FirstName",
        *          "lastName": "John LastName",
        *          "isActive": true,
        *          "isVerifyRequired": true,
        *          "phones":
        *          [
        *             {
        *                 "phoneNumber": "123-123-1233",
        *                 "phoneType": "mobile"
        *             }
        *          ]
        *       }
        *   ]
        *   
        * @apiUse UnauthorizedError
        * @apiUse NotFoundError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ViewUser)]
        [HttpGet]
        [Route("users/{userId}", Name = "GetUserAsync")]
        public async Task<IHttpActionResult> GetUser(string userId)
        {
            var result = await _userService.GetUserAsync(userId);
            return Ok(result);
        }

        /**
        * @api {get} /v1/users/:userId/roles Get user roles
        * @apiVersion 0.1.0
        * @apiName User
        * @apiGroup User Role
        *
        * @apiPermission app.permission->list.users
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
        *                   "claimType": "app.permission",
        *                   "claimValue": "login"
        *               },
        *               {
        *                   "claimType": "app.permission",
        *                   "claimValue": "list.user-claims"
        *               }
        *   ]
        *
        *
        * @apiUse UnauthorizedError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ListUsers)]
        [Route("users/{userId}/roles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserRoles(string userId)
        {
            var result = await _userService.GetUserRolesAsync(userId);
            return Ok(result);
        }

        /**
        * @api {get} /v1/users/:userId/claims Get user claims
        * @apiVersion 0.1.0
        * @apiName Claims
        * @apiGroup User Claim
        *
        * @apiPermission app.permission->list.user-claims
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
        *           "claimType": "app.permission",
        *           "claimValue": "login"
        *       },
        *       {
        *           "claimType": "app.permission",
        *           "claimValue": "list.user-claims"
        *       }
        *   ]
        *
        * @apiUse UnauthorizedError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ListUserClaims)]
        [HttpGet]
        [Route("users/{userId}/claims")]
        public async Task<IHttpActionResult> GetClaims(string userId)
        {
            var result = await _userService.GetUserClaimsAsync(userId);
            return Ok(result);
        }

        /**
        * @api {post} /v1/users/:userId/roles Assign role to user
        * @apiVersion 0.1.0
        * @apiName AssignRole
        * @apiGroup User Role
        *
        * @apiPermission app.permission->assign.role
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
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.AssignRole)]
        [HttpPost]
        [Route("users/{userId}/roles")]
        public async Task<IHttpActionResult> AssignRole([FromUri] string userId, RoleModel roleModel)
        {
            var result = await _userService.AssignUserRoleAsync(userId, roleModel);
            return CreatedAtRoute("GetUserRoleById", new { UserId = userId, RoleId = result.Id }, result);
        }

        /**
        * @api {get} /v1/users/:userId/roles/:roleId Get role
        * @apiVersion 0.1.0
        * @apiName GetUserRoleById
        * @apiGroup User Role
        *
        * @apiPermission app.permission->view.role
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
        *                   "claimType": "app.permission",
        *                   "claimValue": "login"
        *               },
        *               {
        *                   "claimType": "app.permission",
        *                   "claimValue": "list.user-claims"
        *               }
        *       }
        *
        *
        * @apiUse UnauthorizedError
        **/
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.ViewRole)]
        [HttpGet]
        [Route("users/{userId}/roles/{roleId}", Name = "GetUserRoleById")]
        public IHttpActionResult GetUserRoleById(string userId, string roleId)
        {
            var result = _userService.GetUserRoleById(userId, roleId);
            return Ok(result);
        }

        /**
        * @api {delete} /v1/users/:userId/roles/:roleId Remove role from user
        * @apiVersion 0.1.0
        * @apiName RevokeRole
        * @apiGroup User Role
        *
        * @apiPermission app.permission->revoke.role
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
        [ClaimAuthorize(ClaimValue = Constants.AppClaims.RevokeRole)]
        [HttpDelete]
        [Route("users/{userId}/roles/{roleId}")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string userId, [FromUri] string roleId)
        {
            var result = await _userService.RemoveUserFromRoleAsync(userId, roleId);
            return Ok(result);
        }
    }
}
