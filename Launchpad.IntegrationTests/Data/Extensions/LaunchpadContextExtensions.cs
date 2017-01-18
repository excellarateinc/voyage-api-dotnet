using System.IO;

using Launchpad.Data;
using Launchpad.Models.Entities;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.IntegrationTests.Data.Extensions
{
    public static class LaunchpadContextExtensions
    {
        public static ApplicationRole AddRole(this LaunchpadDataContext context)
        {
            var role = new ApplicationRole { Name = Path.GetRandomFileName() };
            var roleStore = new RoleStore<ApplicationRole>(context);
            roleStore.CreateAsync(role).Wait();
            return role;
        }

        public static RoleClaim AddRoleClaim(this LaunchpadDataContext context, ApplicationRole role, string type = "type1", string value = "value1")
        {
            var roleClaim = new RoleClaim
            {
                ClaimType = "type1",
                ClaimValue = "value1",
                RoleId = role.Id
            };
            context.RoleClaims.Add(roleClaim);
            context.SaveChanges();
            return roleClaim;
        }
    }
}
