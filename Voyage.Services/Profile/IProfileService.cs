using System.Threading.Tasks;
using Voyage.Models;

namespace Voyage.Services.Profile
{
    public interface IProfileService
    {
        Task<CurrentUserModel> GetCurrentUserAync(string userId);

        Task<CurrentUserModel> UpdateProfileAsync(string userId, ProfileModel model);

        Task<string> GetProfileImage(string userId);

        Task GetInitialProfileImageAsync(string userId, string emailAddress);
    }
}
