using AutoMapper;
using Launchpad.Core;
using Launchpad.Data.Interfaces;
using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using Launchpad.Services.IdentityManagers;
using Launchpad.Services.Interfaces;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Launchpad.Services
{
    public class RoleService : EntityResultService, IRoleService
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly IMapper _mapper;

        public RoleService(ApplicationRoleManager roleManager, IRoleClaimRepository roleClaimRepository, IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
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
        public EntityResult<RoleModel> GetRoleById(string id)
        {
            // Attempt to find the role by id
            var role = _roleManager.FindById(id);

            return role == null ?
                NotFound<RoleModel>(id) :
                Success(_mapper.Map<RoleModel>(role));
        }

        public async Task<EntityResult<ClaimModel>> AddClaimAsync(string roleId, ClaimModel claim)
        {
            EntityResult<ClaimModel> entityResult;
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity != null)
            {

                var roleClaim = new RoleClaim
                {
                    RoleId = roleEntity.Id,
                    ClaimValue = claim.ClaimValue,
                    ClaimType = claim.ClaimType
                };

                using (var scope = UnitOfWork.Begin())
                {
                    _roleClaimRepository.Add(roleClaim);
                    UnitOfWork.SaveChanges();
                    scope.Commit();
                }
                entityResult = Success(_mapper.Map<ClaimModel>(roleClaim));
            }
            else
            {
                entityResult = NotFound<ClaimModel>(roleId);
            }

            return entityResult;
        }

        public async Task<EntityResult> RemoveRoleAsync(string roleId)
        {
            EntityResult result;
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity != null)
            {
                using (var scope = UnitOfWork.Begin())
                {
                    var identityResult = await _roleManager.DeleteAsync(roleEntity);
                    result = FromIdentityResult(identityResult);
                    scope.Commit();
                }
            }
            else
            {
                result = NotFound(roleId);
            }

            return result;
        }

        public async Task<EntityResult<RoleModel>> CreateRoleAsync(RoleModel model)
        {
            // Create the role
            var role = new ApplicationRole { Name = model.Name };
            using (var scope = UnitOfWork.Begin())
            {
                var identityResult = await _roleManager.CreateAsync(role);
                scope.Commit();
                // Get the role to return as part of the response
                var entityResult = GetRoleByName(role.Name);

                return FromIdentityResult(identityResult, _mapper.Map<RoleModel>(entityResult.Model));
            }
        }

        public EntityResult<IEnumerable<RoleModel>> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Success(_mapper.Map<IEnumerable<RoleModel>>(roles));
        }

        public EntityResult<IEnumerable<ClaimModel>> GetRoleClaims(string name)
        {
            var claims = _roleClaimRepository.GetClaimsByRole(name);
            return Success(_mapper.Map<IEnumerable<ClaimModel>>(claims));
        }

        public EntityResult<IEnumerable<ClaimModel>> GetRoleClaimsByRoleId(string id)
        {
            // Take advantage of queryable
            var claims = _roleClaimRepository.GetAll()
                .Where(_ => _.RoleId == id)
                .ToList();
            return Success(_mapper.Map<IEnumerable<ClaimModel>>(claims));
        }

        public EntityResult RemoveClaim(string roleId, int claimId)
        {
            using (var scope = UnitOfWork.Begin())
            {
                // With the current model, the claim id uniquely identifies the RoleClaim
                // It is not normalized - the record contains the RoleId and the complete definition of the claim
                // This means something like a "login" claim is repeated for each role
                _roleClaimRepository.Delete(claimId);
                UnitOfWork.SaveChanges();
                scope.Commit();
            }
            return Success();
        }

        public EntityResult<ClaimModel> GetClaimById(string roleId, int claimId)
        {
            var claim = _roleClaimRepository.Get(claimId);

            return claim == null ? NotFound<ClaimModel>(claimId) : Success(_mapper.Map<ClaimModel>(claim));
        }

        public EntityResult<RoleModel> GetRoleByName(string name)
        {
            var role = _roleManager.FindByName(name);
            return role == null ? NotFound<RoleModel>(name) : Success(_mapper.Map<RoleModel>(role));
        }
    }
}
