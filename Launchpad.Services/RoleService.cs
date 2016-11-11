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

        public RoleModel GetRoleById(string id)
        {
            return _mapper.Map<RoleModel>(_roleManager.FindById(id));
        }

        public async Task<ClaimModel> AddClaimAsync(string roleId, ClaimModel claim)
        {
            ClaimModel model = null;
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if(roleEntity != null)
            {
                var roleClaim = new RoleClaim { RoleId = roleEntity.Id, ClaimValue = claim.ClaimValue, ClaimType = claim.ClaimType };
                _roleClaimRepository.Add(roleClaim);
                model = _mapper.Map<ClaimModel>(roleClaim);
                   
            }
            return model;
        }

        public async Task<IdentityResult> RemoveRoleAsync(string roleId)
        {
            IdentityResult result;
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity != null)
            {
                result = await _roleManager.DeleteAsync(roleEntity);
            }else
            {
                result = new IdentityResult($"Unable to find role with ID {roleId}");
            }
            return result;
        }

        public async Task<IdentityResult<RoleModel>> CreateRoleAsync(RoleModel model)
        {
            var role = new ApplicationRole();
            role.Name = model.Name;

            var result = await _roleManager.CreateAsync(role);

            var hydratedRole = GetRoleByName(role.Name);
            return new IdentityResult<RoleModel>(result, _mapper.Map<RoleModel>(hydratedRole));
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

        public IEnumerable<ClaimModel> GetRoleClaimsByRoleId(string id)
        {
            //Take advantage of queryable
            var claims = _roleClaimRepository.GetAll()
                .Where(_ => _.RoleId == id)
                .ToList();
            return _mapper.Map<IEnumerable<ClaimModel>>(claims);

        }

        public void RemoveClaim(string roleId, int claimId)
        {
            //With the current model, the claim id uniquely identifies the RoleClaim
            //It is not normalized - the record contains the RoleId and the complete definition of the claim
            //This means something like a "login" claim is repeated for each role
            _roleClaimRepository.Delete(claimId);
        }

        public ClaimModel GetClaimById(string roleId, int claimId)
        {
            return _mapper.Map<ClaimModel>(_roleClaimRepository.Get(claimId));  
        }

        public RoleModel GetRoleByName(string name)
        {
            var role = _roleManager.FindByName(name);
            return _mapper.Map<RoleModel>(role);
        }
    }
}
