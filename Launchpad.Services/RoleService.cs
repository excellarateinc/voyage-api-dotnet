using Launchpad.Services.Interfaces;
using System.Threading.Tasks;
using Launchpad.Models;
using Launchpad.Services.IdentityManagers;
using Microsoft.AspNet.Identity;
using Launchpad.Core;
using Launchpad.Models.EntityFramework;
using Launchpad.Data.Interfaces;
using System;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;

namespace Launchpad.Services
{
    public class RoleService : IRoleService
    {
        private ApplicationRoleManager _roleManager;
        private IRoleClaimRepository _roleClaimRepository;
        private IMapper _mapper;

        public RoleService(ApplicationRoleManager roleManager, IRoleClaimRepository roleClaimRepository, IMapper mapper)
        {
            _roleManager = roleManager.ThrowIfNull(nameof(roleManager));
            _roleClaimRepository = roleClaimRepository.ThrowIfNull(nameof(roleClaimRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public async Task AddClaimAsync(RoleModel role, ClaimModel claim)
        {
            var roleEntity = await _roleManager.FindByNameAsync(role.Name);
            if(roleEntity != null)
            {
                _roleClaimRepository.Add(new RoleClaim { RoleId = roleEntity.Id, ClaimValue = claim.ClaimValue, ClaimType = claim.ClaimType });   
            }
        }

        public async Task<IdentityResult> RemoveRoleAsync(RoleModel role)
        {
            IdentityResult result;
            var roleEntity = await _roleManager.FindByNameAsync(role.Name);
            if (roleEntity != null)
            {
                result = await _roleManager.DeleteAsync(roleEntity);
            }else
            {
                result = new IdentityResult($"Unable to find role with name {role.Name}");
            }
            return result;
        }

        public async Task<IdentityResult> CreateRoleAsync(RoleModel model)
        {
            var role = new ApplicationRole();
            role.Name = model.Name;

            var result = await _roleManager.CreateAsync(role);

            return result;
        }

        public IEnumerable<RoleModel> GetRoles()
        {
            var roles =  _roleManager.Roles.ToList();
            return _mapper.Map<IEnumerable<RoleModel>>(roles);
        }

        public IEnumerable<ClaimModel> GetRoleClaims(string name)
        {
            var claims = _roleClaimRepository.GetClaimsByRole(name);
            return _mapper.Map<IEnumerable<ClaimModel>>(claims);
            
        }

        public void RemoveClaim(string roleName, string claimType, string claimValue)
        {
            var claim = _roleClaimRepository.GetByRoleAndClaim(roleName, claimType, claimValue);
            if (claim != null)
            {
                _roleClaimRepository.Delete(claim.Id);
            }
        }
    }
}
