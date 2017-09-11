using Voyage.Core;
using Voyage.Models;
using System.Threading.Tasks;
using System.Web.Http;
using Voyage.Services.Profile;
using Voyage.Services.User;
using Swashbuckle.Swagger.Annotations;

namespace Voyage.Api.UserManager.API.V1
{
    /// <summary>
    /// Controller that handles user registration.
    /// </summary>
    [RoutePrefix(RoutePrefixConstants.RoutePrefixes.V1)]
    public class AccountController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IProfileService _profileService;

        /// <summary>
        /// Constructor for Account Controller.
        /// </summary>
        public AccountController(IUserService userService, IProfileService profileService)
        {
            _userService = userService.ThrowIfNull(nameof(userService));
            _profileService = profileService.ThrowIfNull(nameof(profileService));
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost]
        [Route("accounts")]
        [SwaggerResponse(201, "UserModel", typeof(UserModel))]
        [SwaggerResponse(400, "BadRequestException")]
        public async Task<IHttpActionResult> Register(RegistrationModel model)
        {
            var result = await _userService.RegisterAsync(model);
            await _profileService.GetInitialProfileImageAsync(result.Id, result.Email);
            return CreatedAtRoute("GetUserAsync", new { userId = result.Id }, result);
        }
    }
}
