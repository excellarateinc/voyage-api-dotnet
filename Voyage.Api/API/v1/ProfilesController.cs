using System.IdentityModel.Claims;
using Voyage.Core;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Voyage.Models;
using Voyage.Services.Profile;
using Swashbuckle.Swagger.Annotations;
using Voyage.Services.User;

namespace Voyage.Api.API.V1
{
    /// <summary>
    /// Controller that handles user session actions.
    /// </summary>
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class ProfilesController : ApiController
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IProfileService _profileService;
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor for the Profile Controller.
        /// </summary>
        public ProfilesController(
            IAuthenticationManager authenticationManager,
            IProfileService profileService,
            IUserService userService)
        {
            _authenticationManager = authenticationManager.ThrowIfNull(nameof(authenticationManager));
            _profileService = profileService.ThrowIfNull(nameof(profileService));
            _userService = userService.ThrowIfNull(nameof(userService));
        }

        /// <summary>
        /// Gets the current user's profile.
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("profiles/me")]
        [SwaggerResponse(200, "CurrentUserModel", typeof(CurrentUserModel))]
        [SwaggerResponse(404, "NotFoundException")]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var currentUser = await _profileService.GetCurrentUserAync(userId);
            return Ok(currentUser);
        }

        /// <summary>
        /// Updates the current user's profile.
        /// </summary>
        [Authorize]
        [HttpPut]
        [Route("profiles/me")]
        [SwaggerResponse(200, "CurrentUserModel", typeof(CurrentUserModel))]
        [SwaggerResponse(404, "NotFoundException")]
        public async Task<IHttpActionResult> UpdateProfile(ProfileModel model)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _profileService.UpdateProfileAsync(userId, model);
            return Ok(result);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost]
        [Route("profiles/register")]
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