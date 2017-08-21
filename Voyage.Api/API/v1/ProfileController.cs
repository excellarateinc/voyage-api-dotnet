using System.IdentityModel.Claims;
using Voyage.Core;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Voyage.Models;
using Voyage.Services.Profile;

namespace Voyage.Api.API.V1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class ProfileController : ApiController
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IProfileService _profileService;

        public ProfileController(IAuthenticationManager authenticationManager, IProfileService profileService)
        {
            _authenticationManager = authenticationManager.ThrowIfNull(nameof(authenticationManager));
            _profileService = profileService.ThrowIfNull(nameof(profileService));
        }

        /// <summary>
        /// Gets the current user's profile.
        /// </summary>
        /// <returns>The current user's profile.</returns>
        [Authorize]
        [HttpGet]
        [Route("profiles/me")]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var currentUser = await _profileService.GetCurrentUserAync(userId);
            return Ok(currentUser);
        }

        /// <summary>
        /// Updates the current user's profile.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The users updated profile.</returns>
        [Route("profiles/me")]
        [Authorize]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateProfile(ProfileModel model)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _profileService.UpdateProfileAsync(userId, model);
            return Ok(result);
        }
    }
}