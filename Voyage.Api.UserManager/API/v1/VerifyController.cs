using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Core;
using Voyage.Models;
using Voyage.Services.Phone;
using Voyage.Services.User;

namespace Voyage.Api.UserManager.API.V1
{
    [RoutePrefix(RoutePrefixConstants.RoutePrefixes.V1)]
    public class VerifyController : ApiController
    {
        private readonly IPhoneService _phoneService;
        private readonly IUserService _userService;

        public VerifyController(IPhoneService phoneService, IUserService userService)
        {
            _phoneService = phoneService.ThrowIfNull(nameof(phoneService));
            _userService = userService.ThrowIfNull(nameof(userService));
        }

        /**
        * @api {get} /v1/Verify/send Send verify code
        * @apiVersion 0.1.0
        * @apiName PostVerifySend
        * @apiGroup Verify
        *
        * @apiPermission authorized
        *
        * @apiUse AuthHeader
        *
        * @apiSuccessExample Success-Response:
        *   HTTP/1.1 204 NO CONTENT
        *
        * @apiUse UnauthorizedError
        **/
        [Authorize]
        [HttpGet]
        [Route("verify/send")]
        public async Task<IHttpActionResult> SendVerificationCode()
        {
            await _phoneService.SendSecurityCodeToUserPhoneNumber(User.Identity.Name); 
            return Ok();
        }

        /**
       * @api {get} /v1/verify verify user
       * @apiVersion 0.1.0
       * @apiName PostVerify
       * @apiGroup Verify
       *
       * @apiPermission authorized
       *
       * @apiUse AuthHeader
       * @apiParam {String} code The code that was delivered to user via the /verify/send method
       *
       * @apiSuccessExample Success-Response:
       *   HTTP/1.1 204 NO CONTENT
       *
       * @apiUse UnauthorizedError
       **/
        [Authorize]
        [HttpPost]
        [Route("verify")]
        public async Task<IHttpActionResult> Verify([FromBody] PhoneSecurityCodeModel phoneSecurityCodeModel)
        {
            var userName = await _userService.GetUserByNameAsync(User.Identity.Name);
            await _phoneService.IsValidSecurityCodeAsync(userName.Id, phoneSecurityCodeModel.Code);

            return Ok();
        }
    }
}