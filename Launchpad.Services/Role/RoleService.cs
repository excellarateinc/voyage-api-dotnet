using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Launchpad.Core;
using Launchpad.Core.Exceptions;
using Launchpad.Data.Repositories.RoleClaim;
using Launchpad.Models;
using Launchpad.Models.Entities;
using Launchpad.Services.IdentityManagers;
using Microsoft.AspNet.Identity;

namespace Launchpad.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly IMapper _mapper;

        public RoleService(ApplicationRoleManager roleManager, IRoleClaimRepository roleClaimRepository, IMapper mapper)
        {
            _roleManager = roleManager.ThrowIfNull(nameof(roleManager));
            _roleClaimRepository = roleClaimRepository.ThrowIfNull(nameof(roleClaimRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        /// <summary>
        /// Find a role by ID
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>EnttiyResult</returns>
        public RoleModel GetRoleById(string id)
        {            
            var role = _roleManager.FindById(id);

            if (role == null)
                throw new NotFoundException($"{Models.Constants.ErrorCodes.EntityNotFound}::Could not locate entity with ID {id}");

            return _mapper.Map<RoleModel>(role);
        }

        public async Task<ClaimModel> AddClaimAsync(string roleId, ClaimModel claim)
        {
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity == null)
                throw new NotFoundException($"{Models.Constants.ErrorCodes.EntityNotFound}::Could not locate entity with ID {roleId}");

            var roleClaim = new RoleClaim
            {
                RoleId = roleEntity.Id,
                ClaimValue = claim.ClaimValue,
                ClaimType = claim.ClaimType
            };
            _roleClaimRepository.Add(roleClaim);
            return _mapper.Map<ClaimModel>(roleClaim);
        }

        public async Task<IdentityResult> RemoveRoleAsync(string roleId)
        {            
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity == null)
                throw new NotFoundException($"{Models.Constants.ErrorCodes.EntityNotFound}::Could not locate entity with ID {roleId}");

            var identityResult = await _roleManager.DeleteAsync(roleEntity);
            return identityResult;
        }

        public async Task<RoleModel> CreateRoleAsync(RoleModel model)
        {
            // Create the role
            var role = new ApplicationRole { Name = model.Name };
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new BadRequestException();
            }

            // Get the role to return as part of the response
            var roleModel = GetRoleByName(role.Name);
            return roleModel;
        }

        public IEnumerable<RoleModel> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return _mapper.Map<IEnumerable<RoleModel>>(roles);
        }

        public IEnumerable<ClaimModel> GetRoleClaims(string name)
        {
            var claims = _roleClaimRepository.GetClaimsByRole(name);
            return _mapper.Map<IEnumerable<ClaimModel>>(claims);
        }

        public IEnumerable<ClaimModel> GetRoleClaimsByRoleId(string id)
        {            
            var claims = _roleClaimRepository.GetAll()
                .Where(_ => _.RoleId == id)
                .ToList();
            return _mapper.Map<IEnumerable<ClaimModel>>(claims);
        }

        public void RemoveClaim(string roleId, int claimId)
        {
            // With the current model, the claim id uniquely identifies the RoleClaim
            // It is not normalized - the record contains the RoleId and the complete definition of the claim
            // This means something like a "login" claim is repeated for each role
            _roleClaimRepository.Delete(claimId);
        }

        public ClaimModel GetClaimById(string roleId, int claimId)
        {
            var claim = _roleClaimRepository.Get(claimId);
            if (claim == null)
                throw new NotFoundException($"{Models.Constants.ErrorCodes.EntityNotFound}::Could not locate entity with ID {roleId}");

            return _mapper.Map<ClaimModel>(claim);
        }

        public RoleModel GetRoleByName(string name)
        {
            var role = _roleManager.FindByName(name);
            if (role == null)
                throw new NotFoundException($"{Models.Constants.ErrorCodes.EntityNotFound}::Could not locate entity with ID {name}");

            return _mapper.Map<RoleModel>(role);
        }
    }
}
