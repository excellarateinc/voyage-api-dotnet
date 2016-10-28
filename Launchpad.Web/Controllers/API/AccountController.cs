using Launchpad.Core;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using Launchpad.Web.AppIdentity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace Launchpad.Web.Controllers.API
{


    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager)
        {
            _userManager = userManager.ThrowIfNull(nameof(userManager));
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
            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();

        }
    }
}
