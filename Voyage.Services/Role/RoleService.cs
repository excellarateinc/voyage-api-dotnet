using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Data.Repositories.RoleClaim;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.IdentityManagers;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Voyage.Services.Role
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

        public RoleModel GetRoleById(string id)
        {
            var role = _roleManager.FindById(id);

            if (role == null)
                throw new NotFoundException($"Could not locate entity with Id {id}");

            return _mapper.Map<RoleModel>(role);
        }

        public async Task<ClaimModel> AddClaimAsync(string roleId, ClaimModel claim)
        {
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity == null)
                throw new NotFoundException($"Could not locate entity with Id {roleId}");

            var roleClaim = new RoleClaim
            {
                RoleId = roleEntity.Id,
                ClaimValue = claim.ClaimValue,
                ClaimType = claim.ClaimType
            };
            await _roleClaimRepository.AddAsync(roleClaim);
            return _mapper.Map<ClaimModel>(roleClaim);
        }

        public async Task<IdentityResult> RemoveRoleAsync(string roleId)
        {
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity == null)
                throw new NotFoundException($"Could not locate entity with Id {roleId}");

            var identityResult = await _roleManager.DeleteAsync(roleEntity);
            return identityResult;
        }

        public async Task<RoleModel> CreateRoleAsync(RoleModel model)
        {
            // Create the role
            var role = new ApplicationRole { Name = model.Name, Description = model.Description ?? string.Empty };
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new BadRequestException();
            }

            // Get the role to return as part of the response
            var roleModel = await GetRoleByNameAsync(role.Name);
            return roleModel;
        }

        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleModel>>(roles);
        }

        public async Task<IEnumerable<ClaimModel>> GetRoleClaimsAsync(string name)
        {
            var claims = await _roleClaimRepository.GetClaimsByRole(name).ToListAsync();
            return _mapper.Map<IEnumerable<ClaimModel>>(claims);
        }

        public async Task<IEnumerable<ClaimModel>> GetRoleClaimsByRoleIdAsync(string id)
        {
            var claims = await _roleClaimRepository.GetAll()
                .Where(_ => _.RoleId == id)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ClaimModel>>(claims);
        }

        public async Task RemoveClaimAsync(string roleId, int claimId)
        {
            // With the current model, the claim id uniquely identifies the RoleClaim
            // It is not normalized - the record contains the RoleId and the complete definition of the claim
            // This means something like a "login" claim is repeated for each role
            await _roleClaimRepository.DeleteAsync(claimId);
        }

        public async Task<ClaimModel> GetClaimByIdAsync(string roleId, int claimId)
        {
            var claim = await _roleClaimRepository.GetAsync(claimId);
            if (claim == null)
                throw new NotFoundException($"Could not locate entity with Id {roleId}");

            return _mapper.Map<ClaimModel>(claim);
        }

        public async Task<RoleModel> GetRoleByNameAsync(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null)
                throw new NotFoundException($"Could not locate entity with Id {name}");

            return _mapper.Map<RoleModel>(role);
        }
    }
}
