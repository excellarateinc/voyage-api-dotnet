using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.UserPhone;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.IdentityManagers;
using Voyage.Services.Role;
using System.Configuration;
using Voyage.Models.Enum;
using System.Data.Entity;

namespace Voyage.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserPhoneRepository _phoneRepository;
        private readonly IRoleService _roleService;
        private readonly ApplicationUserManager _userManager;
        private readonly IMapper _mapper;

        public UserService(
            ApplicationUserManager userManager,
            IMapper mapper,
            IRoleService roleService,
            IUserPhoneRepository phoneRepository)
        {
            _userManager = userManager.ThrowIfNull(nameof(userManager));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _roleService = roleService.ThrowIfNull(nameof(roleService));
            _phoneRepository = phoneRepository.ThrowIfNull(nameof(phoneRepository));
            _userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromDays(Convert.ToDouble(ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"]));
            _userManager.MaxFailedAccessAttemptsBeforeLockout = Convert.ToInt16(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]);
            _userManager.UserLockoutEnabledByDefault = Convert.ToBoolean(ConfigurationManager.AppSettings["UserLockoutEnabledByDefault"]);
        }

        public async Task<UserModel> UpdateUserAsync(string userId, UserModel model)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null)
                throw new NotFoundException($"Could not locate entity with Id {userId}");

            if (!IsValidPhoneNumbers(model))
            {
                throw new BadRequestException(HttpStatusCode.BadRequest.ToString(), "Invalid phone number.");
            }

            _mapper.Map<UserModel, ApplicationUser>(model, appUser);

            CollectionHelpers.MergeCollection(
                _mapper,
                source: model.Phones,
                destination: appUser.Phones,
                predicate: (s, d) => s.Id == d.Id,
                deleteAction: entity => _phoneRepository.Delete(entity.Id));

            await _userManager.UpdateAsync(appUser);
            return _mapper.Map<UserModel>(appUser);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleId)
        {
            var roleModel = _roleService.GetRoleById(roleId);
            if (roleModel == null)
            {
                return null;
            }

            var result = await _userManager.RemoveFromRoleAsync(userId, roleModel.Name);
            return result;
        }

        public async Task<RoleModel> AssignUserRoleAsync(string userId, RoleModel roleModel)
        {
            var identityResult = await _userManager.AddToRoleAsync(userId, roleModel.Name);
            if (!identityResult.Succeeded)
                throw new BadRequestException();

            return await _roleService.GetRoleByNameAsync(roleModel.Name);
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.Where(user => !user.Deleted).ToListAsync();
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<bool> IsValidCredentialAsync(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user != null && user.IsActive && !user.Deleted;
        }

        public async Task<UserModel> CreateUserAsync(UserModel model)
        {
            var appUser = new ApplicationUser { IsActive = true, IsVerifyRequired = true };
            _mapper.Map<UserModel, ApplicationUser>(model, appUser);
            IdentityResult result = await _userManager.CreateAsync(appUser, "Hello123!");
            if (!result.Succeeded)
                throw new BadRequestException();

            return _mapper.Map<UserModel>(appUser);
        }

        public async Task<UserModel> RegisterAsync(RegistrationModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phones = model.Phones.Select(phone => new UserPhone
                {
                    PhoneNumber = _phoneRepository.GetE164Format(phone.PhoneNumber),
                    PhoneType = (PhoneType)Enum.Parse(typeof(PhoneType), phone.PhoneType),
                    UserId = new ApplicationUser().Id
                }).ToList(),
                IsActive = true,
                IsVerifyRequired = true,
                LockoutEnabled = true
            };
            var appUser = await _userManager.FindByNameAsync(user.UserName);
            if (appUser != null)
            {
                if (appUser.UserName == user.UserName)
                    throw new BadRequestException(HttpStatusCode.BadRequest.ToString(), "User already exists with the username. Please choose a different username");
            }

            var identityResult = await _userManager.CreateAsync(user, model.Password);
            if (!identityResult.Succeeded)
                throw new BadRequestException(string.Join(Environment.NewLine, identityResult.Errors));

            identityResult = await _userManager.AddToRoleAsync(user.Id, "Basic");
            if (!identityResult.Succeeded)
                throw new BadRequestException(string.Join(Environment.NewLine, identityResult.Errors));

            var userModel = new UserModel();
            _mapper.Map<ApplicationUser, UserModel>(user, userModel);

            return userModel;
        }

        public async Task<IEnumerable<ClaimModel>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Could not locate entity with Id {userId}");

            var identity = await CreateClaimsIdentityAsync(user.UserName, "OAuth");
            return _mapper.Map<IEnumerable<ClaimModel>>(identity.Claims);
        }

        public async Task<ClaimsIdentity> CreateClaimsIdentityAsync(string userName, string authenticationType)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException($"Could not locate entity with Id {userName}");

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await _userManager.CreateIdentityAsync(user, authenticationType);

            // Add in role claims
            var userRoles = _userManager.GetRoles(user.Id);
            foreach (var role in userRoles)
            {
                var claims = await _roleService.GetRoleClaimsAsync(role);
                userIdentity.AddClaims(claims.Select(_ => new Claim(_.ClaimType, _.ClaimValue)));
            }

            return userIdentity;
        }

        public async Task<ClaimsIdentity> CreateJwtClaimsIdentityAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundException($"Could not locate entity with Id {userName}");

            var identity = new ClaimsIdentity("JWT");

            // Add in role claims
            var userRoles = _userManager.GetRoles(user.Id);
            foreach (var role in userRoles)
            {
                var claims = await _roleService.GetRoleClaimsAsync(role);
                identity.AddClaims(claims.Select(_ => new Claim(_.ClaimType, _.ClaimValue)));
            }

            return identity;
        }

        /// <summary>
        /// This create client claims based on test data. Your application will need to create claim based on your business rule
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> CreateClientClaimsIdentityAsync(string clientId)
        {
            // TODO: This create client claims based on test data.Your application will need to create claim based on your business rule
            var identity = new ClaimsIdentity("JWT");
            if (Clients.Client2.Id == clientId || Clients.Client1.Id == clientId)
            {
                // Add in role claims
                var userRoles = new List<string> { "Administrator" };
                foreach (var role in userRoles)
                {
                    var claims = await _roleService.GetRoleClaimsAsync(role);
                    identity.AddClaims(claims.Select(_ => new Claim(_.ClaimType, _.ClaimValue)));
                }

                var client = Clients.Client1.Id == clientId ? Clients.Client1 : Clients.Client2;
                identity.AddClaim(new Claim("client_id", client.Id));
                identity.AddClaim(new Claim("client_secret", client.Secret));
            }

            return identity;
        }

        public async Task<IEnumerable<RoleModel>> GetUserRolesAsync(string userId)
        {
            var roles = await _userManager.GetRolesAsync(userId);
            var roleModels = (await _roleService.GetRolesAsync()).Where(_ => roles.Contains(_.Name));
            return roleModels;
        }

        public RoleModel GetUserRoleById(string userId, string roleId)
        {
            var roleModel = _roleService.GetRoleById(roleId);
            if (roleModel == null)
                throw new NotFoundException($"Could not locate entity with Id {roleId}");

            if (!_userManager.IsInRole(userId, roleModel.Name))
                throw new UnauthorizedException($"User not authorized for role {roleModel.Name}");

            return roleModel;
        }

        public async Task<UserModel> GetUserAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null || appUser.Deleted)
                throw new NotFoundException($"Could not locate entity with ID {userId}");

            return _mapper.Map<UserModel>(appUser);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null)
                throw new NotFoundException($"Could not locate entity with ID {userId}");

            var identityResult = await _userManager.DeleteAsync(appUser);
            return identityResult;
        }

        public async Task<UserModel> GetUserByNameAsync(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null || appUser.Deleted)
                throw new NotFoundException($"Could not locate entity with Name {userName}");

            return _mapper.Map<UserModel>(appUser);
        }

        public async Task<IdentityResult> ChangePassword(string userId, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public async Task<IdentityResult> ResetPassword(string userId, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(userId, token, newPassword);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userName)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(userName);
        }

        private bool IsValidPhoneNumbers(UserModel userModel)
        {
            // validate user phone numbers
            var isValidPhoneNumbers = true;
            foreach (var phone in userModel.Phones)
            {
                string formatedPhoneNumber;
                if (_phoneRepository.IsValidPhoneNumber(phone.PhoneNumber, out formatedPhoneNumber))
                {
                    phone.PhoneNumber = formatedPhoneNumber;
                }
                else
                {
                    isValidPhoneNumbers = false;
                    break;
                }
            }

            return isValidPhoneNumbers;
        }

        public async Task AccessFailedAsync(string userId)
        {
            await _userManager.AccessFailedAsync(userId);
        }

        public async Task ResetAccessFailedCountAsync(string userId)
        {
            await _userManager.ResetAccessFailedCountAsync(userId);
        }

        public async Task<bool> IsLockedOutAsync(string userId)
        {
            return await _userManager.IsLockedOutAsync(userId);
        }

        public async Task SetLockoutEndDateAsync(string userId)
        {
            await _userManager.SetLockoutEndDateAsync(userId, DateTime.UtcNow);
        }
    }
}
