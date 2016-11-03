using System.Collections.Generic;

namespace Launchpad.Models
{
    public class UserWithRolesModel : UserModel
    {
        public IEnumerable<RoleModel> Roles { get; set; }
    }
}
