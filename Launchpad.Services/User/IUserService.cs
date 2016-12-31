﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Launchpad.Models;
using Microsoft.AspNet.Identity;

namespace Launchpad.Services.User
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegistrationModel model);

        Task<UserModel> CreateUserAsync(UserModel model);

        Task<bool> IsValidCredential(string userName, string password);

        Task<ClaimsIdentity> CreateClaimsIdentityAsync(string userName, string authenticationType);

        IEnumerable<UserModel> GetUsers();

        Task<RoleModel> AssignUserRoleAsync(string userId, RoleModel roleModel);

        Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleId);

        Task<IEnumerable<RoleModel>> GetUserRolesAsync(string userId);

        Task<IEnumerable<ClaimModel>> GetUserClaimsAsync(string userId);

        RoleModel GetUserRoleById(string userId, string roleId);

        Task<UserModel> GetUserAsync(string userId);

        Task<IdentityResult> DeleteUserAsync(string userId);

        Task<UserModel> UpdateUserAsync(string userId, UserModel model);
    }
}
