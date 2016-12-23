using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Launchpad.Models;

namespace Launchpad.Services.User
{
    public interface IUserService
    {
        Task<EntityResult> RegisterAsync(RegistrationModel model);

        Task<EntityResult<UserModel>> CreateUserAsync(UserModel model);

        Task<bool> IsValidCredential(string userName, string password);

        Task<ClaimsIdentity> CreateClaimsIdentityAsync(string userName, string authenticationType);

        EntityResult<IEnumerable<UserModel>> GetUsers();

        Task<EntityResult<RoleModel>> AssignUserRoleAsync(string userId, RoleModel roleModel);

        Task<EntityResult> RemoveUserFromRoleAsync(string userId, string roleId);

        Task<EntityResult<IEnumerable<RoleModel>>> GetUserRolesAsync(string userId);

        Task<EntityResult<IEnumerable<ClaimModel>>> GetUserClaimsAsync(string userId);

        EntityResult<RoleModel> GetUserRoleById(string userId, string roleId);

        Task<EntityResult<UserModel>> GetUserAsync(string userId);

        Task<EntityResult> DeleteUserAsync(string userId);

        Task<EntityResult<UserModel>> UpdateUserAsync(string userId, UserModel model);
    }
}
