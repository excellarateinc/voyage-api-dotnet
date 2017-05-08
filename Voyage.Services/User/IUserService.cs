using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Voyage.Models;
using Microsoft.AspNet.Identity;

namespace Voyage.Services.User
{
    public interface IUserService
    {
        Task<UserModel> RegisterAsync(RegistrationModel model);

        Task<UserModel> CreateUserAsync(UserModel model);

        Task<bool> IsValidCredential(string userName, string password);

        Task<ClaimsIdentity> CreatePermissionsIdentityAsync(string userName, string authenticationType);

        Task<ClaimsIdentity> CreateJwtPermissionsIdentityAsync(string userName);

        IEnumerable<UserModel> GetUsers();

        Task<RoleModel> AssignUserRoleAsync(string userId, RoleModel roleModel);

        Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleId);

        Task<IEnumerable<RoleModel>> GetUserRolesAsync(string userId);

        Task<IEnumerable<PermissionModel>> GetUserPermissionsAsync(string userId);

        Task<ClaimsIdentity> CreateClientPermissionsIdentityAsync(string clientId);

        RoleModel GetUserRoleById(string userId, string roleId);

        Task<UserModel> GetUserAsync(string userId);

        Task<UserModel> GetUserByNameAsync(string userName);

        Task<IdentityResult> DeleteUserAsync(string userId);

        Task<UserModel> UpdateUserAsync(string userId, UserModel model);

        Task<IdentityResult> ChangePassword(string userId, string token, string newPassword);

        Task<string> GeneratePasswordResetTokenAsync(string userName);
    }
}
