using Launchpad.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Launchpad.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(RoleModel model);
    }
}
