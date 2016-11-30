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
    public abstract class EntityResultService
    {
        protected EntityResult Missing(object id)
        {
            return new EntityResult(false, true)
              .WithMissingEntity(id);
        }

        protected EntityResult<TModel> Missing<TModel>(object id)
            where TModel : class
        {
            return new EntityResult<TModel>(null, false, true)
              .WithMissingEntity(id);
        }


        protected EntityResult<TModel> Success<TModel>(TModel model)
            where TModel : class
        {
            return new EntityResult<TModel>(model, true, false);
        }

        protected EntityResult Success()
        {
            return new EntityResult(true, false);
        }


        protected EntityResult<TModel> FromIdentityResult<TModel>(IdentityResult result, TModel model)
             where TModel : class
        {
            return new EntityResult<TModel>(model, result.Succeeded, false, result.Errors.ToArray());
        }

        protected EntityResult FromIdentityResult(IdentityResult result)
        {
            return new EntityResult(result.Succeeded, false, result.Errors.ToArray());
        }

    }

    public class RoleService : EntityResultService, IRoleService
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

        /// <summary>
        /// Find a role by ID
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>EnttiyResult</returns>
        public EntityResult<RoleModel> GetRoleById(string id)
        {
            //Attempt to find the role by id
            var role = _roleManager.FindById(id);

            return role == null ?
                Missing<RoleModel>(id) :
                Success(_mapper.Map<RoleModel>(role));

        }


        public async Task<EntityResult<ClaimModel>> AddClaimAsync(string roleId, ClaimModel claim)
        {
            EntityResult<ClaimModel> model = null;
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity != null)
            {
                var roleClaim = new RoleClaim
                {
                    RoleId = roleEntity.Id,
                    ClaimValue = claim.ClaimValue,
                    ClaimType = claim.ClaimType
                };
                _roleClaimRepository.Add(roleClaim);
                model = Success(_mapper.Map<ClaimModel>(roleClaim));
            }
            else
            {
                model = Missing<ClaimModel>(roleId);
            }
            return model;
        }

        public async Task<EntityResult> RemoveRoleAsync(string roleId)
        {
            EntityResult result;
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity != null)
            {
                var identityResult = await _roleManager.DeleteAsync(roleEntity);
                result = FromIdentityResult(identityResult);
            }
            else
            {
                result = Missing(roleId);
            }
            return result;
        }

        public async Task<EntityResult<RoleModel>> CreateRoleAsync(RoleModel model)
        {

            //Create the role
            var role = new ApplicationRole();
            role.Name = model.Name;
            var result = await _roleManager.CreateAsync(role);

            //Get the role to return as part of the response
            var entityResult = GetRoleByName(role.Name);

            return FromIdentityResult(result, _mapper.Map<RoleModel>(entityResult.Model));
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
            //Take advantage of queryable
            var claims = _roleClaimRepository.GetAll()
                .Where(_ => _.RoleId == id)
                .ToList();
            return Success(_mapper.Map<IEnumerable<ClaimModel>>(claims));

        }

        public EntityResult RemoveClaim(string roleId, int claimId)
        {
            //With the current model, the claim id uniquely identifies the RoleClaim
            //It is not normalized - the record contains the RoleId and the complete definition of the claim
            //This means something like a "login" claim is repeated for each role
            _roleClaimRepository.Delete(claimId);

            return Success();

        }

        public EntityResult<ClaimModel> GetClaimById(string roleId, int claimId)
        {
            var claim = _roleClaimRepository.Get(claimId);

            return claim == null ? Missing<ClaimModel>(claimId) : Success(_mapper.Map<ClaimModel>(claim));
        }

        public EntityResult<RoleModel> GetRoleByName(string name)
        {
            var role = _roleManager.FindByName(name);
            return role == null ? Missing<RoleModel>(name) : Success(_mapper.Map<RoleModel>(role));
        }
    }
}
