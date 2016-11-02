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

        
        public async Task<IdentityResult> RemoveUserFromRoleAsync(RoleModel roleModel, UserModel userModel)
        {
           
            var result = await _userManager.RemoveFromRoleAsync(userModel.Id, roleModel.Name);
            return result;
        }

        public async Task<IdentityResult> AssignUserRoleAsync(RoleModel roleModel, UserModel userModel)
        {

            var result = await _userManager.AddToRoleAsync(userModel.Id, roleModel.Name);
            return result;
        }

       

        public IEnumerable<UserModel> GetUsers()
        {
            return _mapper.Map<IEnumerable<UserModel>>(_userManager.Users.ToList());
        }

        public async Task<bool> IsValidCredential(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user != null;
        }
        
        public async Task<IdentityResult> RegisterAsync(RegistrationModel model)
        {
            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
          
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user.Id, "Basic");
            }

            return result;
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
    }
}
