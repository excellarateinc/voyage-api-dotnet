using Launchpad.Models.EntityFramework;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Launchpad.Data.IntegrationTests.Extensions
{
    public static class LaunchpadContextExtensions
    {
        public static Widget AddWidget(this LaunchpadDataContext context)
        {
            var widget = new Models.EntityFramework.Widget { Name = "My Test Widget", Color="Blue" };
            context.Widgets.Add(widget);
            context.SaveChanges();
            return widget;
        }

        public static ApplicationRole AddRole(this LaunchpadDataContext context)
        {
            var role = new ApplicationRole() { Name = "MyRoleName" };
            var roleStore = new RoleStore<ApplicationRole>(context);
            roleStore.CreateAsync(role).Wait();
            return role;
        }
        
        public static RoleClaim AddRoleClaim(this LaunchpadDataContext context, ApplicationRole role, string type = "type1", string value = "value1")
        {
            var roleClaim = new RoleClaim();
            roleClaim.ClaimType = "type1";
            roleClaim.ClaimValue = "value1";
            roleClaim.RoleId = role.Id;
            context.RoleClaims.Add(roleClaim);
            context.SaveChanges();
            return roleClaim;
        }
    }
}
