using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Services.Interfaces;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API
{


    [RoutePrefix(Constants.RoutePrefixes.Account)]
    public class AccountController : ApiController
    {
        private IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));
        }

        

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

  


        /**
        * @api {post} /account/register Register a new account
        * @apiVersion 0.1.0
        * @apiName CreateAccount
        * @apiGroup Account
        * 
        * @apiPermission none
        * 
        * @apiParam {String} email User's email
        * @apiParam {String} password User's password
        * @apiParam {String} confirmPassword User's password (x2) 
        * 
        * @apiSuccessExample Success-Response:
        *      HTTP/1.1 200 OK
        *      
        * @apiUse BadRequestError
        */
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegistrationModel model)
        {

            IdentityResult result = await _userService.RegisterAsync(model);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();

        }
    }
}
