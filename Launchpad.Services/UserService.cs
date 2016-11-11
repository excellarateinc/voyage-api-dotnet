using Launchpad.Services.Interfaces;
using System.Threading.Tasks;
using Launchpad.Models;
using Microsoft.AspNet.Identity;
using Launchpad.Services.IdentityManagers;
using Launchpad.Models.EntityFramework;
using Launchpad.Core;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Data.Entity;

namespace Launchpad.Services
{
    public class UserService : IUserService
    {
        private IRoleService _roleService;
        private ApplicationUserManager _userManager;
        private IMapper _mapper;


        public UserService(ApplicationUserManager userManager, IMapper mapper, IRoleService roleService)
        {
            _userManager = userManager.ThrowIfNull(nameof(userManager));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _roleService = roleService.ThrowIfNull(nameof(roleService));
        }

        public async Task<IdentityResult<UserModel>> UpdateUserAsync(string userId, UserModel model)
        {
            var appUser = await _userManager.FindByIdAsync(userId);

            _mapper.Map<UserModel, ApplicationUser>(model, appUser);

            var identityResult = await _userManager.UpdateAsync(appUser);

            return new IdentityResult<UserModel>(identityResult, _mapper.Map<UserModel>(appUser));

        }

            
        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleId)
        {

            var role = _roleService.GetRoleById(roleId);
            var result = await _userManager.RemoveFromRoleAsync(userId, role.Name);
            return result;
        }

        public async Task<IdentityResult<RoleModel>> AssignUserRoleAsync(string userId, RoleModel roleModel)
        {
            var result = await _userManager.AddToRoleAsync(userId, roleModel.Name);
            
            var hydratedRole = _roleService.GetRoleByName(roleModel.Name);
            return new IdentityResult<RoleModel>(IdentityResult.Success, hydratedRole);
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return _mapper.Map<IEnumerable<UserModel>>(_userManager.Users.ToList());
        }

        public async Task<bool> IsValidCredential(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user != null && user.IsActive;
        }
        
        public async Task<IdentityResult<UserModel>> CreateUserAsync(UserModel model)
        {
            var appUser = new ApplicationUser();
            _mapper.Map<UserModel, ApplicationUser>(model, appUser);
            var result = await _userManager.CreateAsync(appUser, "Hello123!");
            return new IdentityResult<UserModel>(result, _mapper.Map<UserModel>(appUser));
        }

        public async Task<IdentityResult> RegisterAsync(RegistrationModel model)
        {
            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, FirstName=model.FirstName, LastName= model.LastName, IsActive = true };
          
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user.Id, "Basic");
            }

            return result;
        }

        public async Task<IEnumerable<ClaimModel>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new ArgumentException($"Unable to find user {userId}");
            }
            var identity = await CreateClaimsIdentityAsync(user.UserName, "OAuth");
            return _mapper.Map<IEnumerable<ClaimModel>>(identity.Claims);
        }

        public async Task<ClaimsIdentity> CreateClaimsIdentityAsync(string userName, string authenticationType)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user == null)
            {
                throw new ArgumentException($"Unable to find user {userName}");
            }

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await _userManager.CreateIdentityAsync(user, authenticationType);

            //Add in role claims
            var userRoles = _userManager.GetRoles(user.Id);
            var roleClaims = userRoles.Select(_ => _roleService.GetRoleClaims(_))
                .SelectMany(_ => _)
                .Select(_ => new Claim(_.ClaimType, _.ClaimValue));
            userIdentity.AddClaims(roleClaims);
                            
            return userIdentity;
        }

        public async Task<IEnumerable<RoleModel>> GetUserRolesAsync(string userId)
        {
           
            var roles = await _userManager.GetRolesAsync(userId);

            //TODO: Refactor this to pass in the role list (IQueryable)
            var roleModels = _roleService.GetRoles()
                            .Where(_ => roles.Contains(_.Name));

            return roleModels;
        }

        public RoleModel GetUserRoleById(string userId, string roleId)
        {
            var role = _roleService.GetRoleById(roleId);
            
            return _userManager.IsInRole(userId, role.Name) ? role : null;

        }

        public async Task<UserModel> GetUserAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            return _mapper.Map<UserModel>(appUser);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            var identityResult = await _userManager.DeleteAsync(appUser);
            return identityResult;
        }
    }
}
