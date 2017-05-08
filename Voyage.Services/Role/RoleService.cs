using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Voyage.Core;
using Voyage.Core.Exceptions;
using Voyage.Models;
using Voyage.Models.Entities;
using Voyage.Services.IdentityManagers;
using Microsoft.AspNet.Identity;
using Voyage.Data.Repositories.RolePermission;

namespace Voyage.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationRoleManager _roleManager;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IMapper _mapper;

        public RoleService(ApplicationRoleManager roleManager, IRolePermissionRepository rolePermissionRepository, IMapper mapper)
        {
            _roleManager = roleManager.ThrowIfNull(nameof(roleManager));
            _rolePermissionRepository = rolePermissionRepository.ThrowIfNull(nameof(rolePermissionRepository));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public RoleModel GetRoleById(string id)
        {
            var role = _roleManager.FindById(id);

            if (role == null)
                throw new NotFoundException($"Could not locate entity with Id {id}");

            return _mapper.Map<RoleModel>(role);
        }

        public async Task<PermissionModel> AddPermissionAsync(string roleId, PermissionModel permission)
        {
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity == null)
                throw new NotFoundException($"Could not locate entity with Id {roleId}");

            var rolePermission = new RolePermission
            {
                RoleId = roleEntity.Id,
                PermissionValue = permission.PermissionValue,
                PermissionType = permission.PermissionType
            };
            _rolePermissionRepository.Add(rolePermission);
            return _mapper.Map<PermissionModel>(rolePermission);
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
            var roleModel = GetRoleByName(role.Name);
            return roleModel;
        }

        public IEnumerable<RoleModel> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return _mapper.Map<IEnumerable<RoleModel>>(roles);
        }

        public IEnumerable<PermissionModel> GetRolePermissions(string name)
        {
            var permissions = _rolePermissionRepository.GetPermissionsByRole(name);
            return _mapper.Map<IEnumerable<PermissionModel>>(permissions);
        }

        public IEnumerable<PermissionModel> GetRolePermissionsByRoleId(string id)
        {
            var permissions = _rolePermissionRepository.GetAll()
                .Where(_ => _.RoleId == id)
                .ToList();
            return _mapper.Map<IEnumerable<PermissionModel>>(permissions);
        }

        public void RemovePermission(string roleId, int permissionId)
        {
            // With the current model, the permission id uniquely identifies the RolePermission
            // It is not normalized - the record contains the RoleId and the complete definition of the permission
            // This means something like a "login" permission is repeated for each role
            _rolePermissionRepository.Delete(permissionId);
        }

        public PermissionModel GetPermissionById(string roleId, int permissionId)
        {
            var permission = _rolePermissionRepository.Get(permissionId);
            if (permission == null)
                throw new NotFoundException($"Could not locate entity with Id {roleId}");

            return _mapper.Map<PermissionModel>(permission);
        }

        public RoleModel GetRoleByName(string name)
        {
            var role = _roleManager.FindByName(name);
            if (role == null)
                throw new NotFoundException($"Could not locate entity with Id {name}");

            return _mapper.Map<RoleModel>(role);
        }
    }
}
