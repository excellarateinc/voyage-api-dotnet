using System;
using Voyage.Core;
using Voyage.Models;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Services.User;

namespace Voyage.Api.API.V1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class AccountController : ApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));
        }

        /**
        * @api {post} /api/v1/profile Create new profile
        * @apiVersion 0.1.0
        * @apiName CreateProfile
        * @apiGroup Profile
        *
        * @apiPermission none
        *
        * @apiParam {String} email User's email
        * @apiParam {String} password User's password
        * @apiParam {String} confirmPassword User's password (x2)
        * @apiParam {String} firstName First name
        * @apiParam {String} lastName Last name
        * @apiSuccess {Object[]} users.phones User phone numbers
        * @apiSuccess {String} users.phones.phoneNumber Phone number
        * @apiSuccess {String} users.phones.phoneType Phone type
        *
        * @apiSuccessExample Success-Response:
        *      HTTP/1.1 201 Created
        *      { "Location" : /api/v1/users/1 } 
        *
        * @apiUse BadRequestError
        */
        [Route("profile")]
        public async Task<IHttpActionResult> Register(RegistrationModel model)
        {
            try
            {
                var result = await _userService.RegisterAsync(model);                
                return CreatedAtRoute("GetUserAsync", new { userId = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
