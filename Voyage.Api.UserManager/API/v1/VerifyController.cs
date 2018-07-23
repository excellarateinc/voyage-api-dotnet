using Swashbuckle.Swagger.Annotations;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Core;
using Voyage.Models;
using Voyage.Services.Phone;
using Voyage.Services.User;

namespace Voyage.Api.UserManager.API.V1
{
    /// <summary>
    /// Controller that handles user verification.
    /// </summary>
    [RoutePrefix(RoutePrefixConstants.RoutePrefixes.V1)]
    public class VerifyController : ApiController
    {
        private readonly IPhoneService _phoneService;
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor for the Verification Controller.
        /// </summary>
        public VerifyController(IPhoneService phoneService, IUserService userService)
        {
            _phoneService = phoneService.ThrowIfNull(nameof(phoneService));
            _userService = userService.ThrowIfNull(nameof(userService));
        }

        /// <summary>
        /// Send verify code
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("verify/send")]
        [SwaggerResponse(204)]
        [SwaggerResponse(401, "UnauthorizedException")]
        public async Task<IHttpActionResult> SendVerificationCode()
        {
            await _phoneService.SendSecurityCodeToUserPhoneNumberAsync(User.Identity.Name); 
            return Ok();
        }

        /// <summary>
        /// Verify user
        /// </summary>
        /// <param name="phoneSecurityCodeModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("verify")]
        [SwaggerResponse(204)]
        [SwaggerResponse(401, "UnauthorizedException")]
        public async Task<IHttpActionResult> Verify([FromBody] PhoneSecurityCodeModel phoneSecurityCodeModel)
        {
            var userName = await _userService.GetUserByNameAsync(User.Identity.Name);
            await _phoneService.IsValidSecurityCodeAsync(userName.Id, phoneSecurityCodeModel.Code);
            return Ok();
        }
    }
}