using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Launchpad.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegistrationModel model);

        Task<bool> IsValidCredential(string userName, string password);

        Task<ClaimsIdentity> CreateClaimsIdentityAsync(string userName, string authenticationType);

        IEnumerable<UserModel> GetUsers();

        Task<IdentityResult<RoleModel>> AssignUserRoleAsync(string userId, RoleModel roleModel);

        Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleId);

        Task<IEnumerable<RoleModel>> GetUserRolesAsync(string userId);

        Task<IEnumerable<ClaimModel>> GetUserClaimsAsync(string userId);

        RoleModel GetUserRoleById(string userId, string roleId);

        Task<UserModel> GetUser(string userId);

        Task<IdentityResult<UserModel>> UpdateUser(string userId, UserModel model);

    }
}
