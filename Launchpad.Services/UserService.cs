using AutoMapper;
using Launchpad.Core;
using Launchpad.Data.Interfaces;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.IdentityManagers;
using Launchpad.Services.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Launchpad.Services
{
    public class UserService : EntityResultService, IUserService
    {
        private readonly IUserPhoneRepository _phoneRepository;
        private readonly IRoleService _roleService;
        private readonly ApplicationUserManager _userManager;
        private readonly IMapper _mapper;

        public UserService(ApplicationUserManager userManager, IMapper mapper, IRoleService roleService, IUserPhoneRepository phoneRepository) : base(mapper)
        {
            _userManager = userManager.ThrowIfNull(nameof(userManager));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _roleService = roleService.ThrowIfNull(nameof(roleService));
            _phoneRepository = phoneRepository.ThrowIfNull(nameof(phoneRepository));
        }

        public async Task<EntityResult<UserModel>> UpdateUserAsync(string userId, UserModel model)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null)
                return NotFound<UserModel>(userId);

            _mapper.Map<UserModel, ApplicationUser>(model, appUser);

            MergeCollection(source: model.Phones,
                destination: appUser.Phones,
                predicate: (s, d) => s.Id == d.Id,
                deleteAction: entity => _phoneRepository.Delete(entity.Id));

            var identityResult = await _userManager.UpdateAsync(appUser);
            return FromIdentityResult(identityResult, _mapper.Map<UserModel>(appUser));
        }

        public async Task<EntityResult> RemoveUserFromRoleAsync(string userId, string roleId)
        {
            var entityResult = _roleService.GetRoleById(roleId);
            if (entityResult.IsEntityNotFound)
            {
                return entityResult;
            }

            var result = await _userManager.RemoveFromRoleAsync(userId, entityResult.Model.Name);
            return FromIdentityResult(result);
        }

        public async Task<EntityResult<RoleModel>> AssignUserRoleAsync(string userId, RoleModel roleModel)
        {
            var identityResult = await _userManager.AddToRoleAsync(userId, roleModel.Name);
            if (identityResult.Succeeded)
            {
                return _roleService.GetRoleByName(roleModel.Name);
            }
            return FromIdentityResult<RoleModel>(identityResult, null);
        }

        public EntityResult<IEnumerable<UserModel>> GetUsers()
        {
            var users = _userManager
                .Users
                .Where(user => !user.Deleted)
                .ToList();
            return Success(_mapper.Map<IEnumerable<UserModel>>(users));
        }

        public async Task<bool> IsValidCredential(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user != null && user.IsActive && !user.Deleted;
        }

        public async Task<EntityResult<UserModel>> CreateUserAsync(UserModel model)
        {
            var appUser = new ApplicationUser();
            _mapper.Map<UserModel, ApplicationUser>(model, appUser);
            var result = await _userManager.CreateAsync(appUser, "Hello123!");
            return FromIdentityResult(result, _mapper.Map<UserModel>(appUser));
        }

        public async Task<EntityResult> RegisterAsync(RegistrationModel model)
        {
            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password);

            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.AddToRoleAsync(user.Id, "Basic");
            }

            return FromIdentityResult(identityResult, user);
        }

        public async Task<EntityResult<IEnumerable<ClaimModel>>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound<IEnumerable<ClaimModel>>(userId);
            }

            var identity = await CreateClaimsIdentityAsync(user.UserName, "OAuth");
            return Success(_mapper.Map<IEnumerable<ClaimModel>>(identity.Claims));
        }

        public async Task<ClaimsIdentity> CreateClaimsIdentityAsync(string userName, string authenticationType)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new ArgumentException($"Unable to find user {userName}");
            }

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await _userManager.CreateIdentityAsync(user, authenticationType);

            //Add in role claims
            var userRoles = _userManager.GetRoles(user.Id);
            var roleClaims = userRoles.Select(_ => _roleService.GetRoleClaims(_))
                .SelectMany(_ => _.Model)
                .Select(_ => new Claim(_.ClaimType, _.ClaimValue));
            userIdentity.AddClaims(roleClaims);

            return userIdentity;
        }

        public async Task<EntityResult<IEnumerable<RoleModel>>> GetUserRolesAsync(string userId)
        {

            var roles = await _userManager.GetRolesAsync(userId);

            //TODO: Refactor this to pass in the role list (IQueryable)
            var roleModels = _roleService.GetRoles()
                            .Model
                            .Where(_ => roles.Contains(_.Name));

            return Success(roleModels);
        }

        public EntityResult<RoleModel> GetUserRoleById(string userId, string roleId)
        {
            var entityResult = _roleService.GetRoleById(roleId);
            if (entityResult.IsEntityNotFound)
            {
                return entityResult;
            }
            if (!_userManager.IsInRole(userId, entityResult.Model.Name))
            {
                return NotFound<RoleModel>(roleId);
            }
            return entityResult;
        }

        public async Task<EntityResult<UserModel>> GetUserAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            return appUser == null || appUser.Deleted
                ? NotFound<UserModel>(userId)
                : Success(_mapper.Map<UserModel>(appUser));
        }

        public async Task<EntityResult> DeleteUserAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null)
            {
                return NotFound(userId);
            }

            var identityResult = await _userManager.DeleteAsync(appUser);
            return FromIdentityResult(identityResult);
        }
    }
}
