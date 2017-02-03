using System.IO;
using Voyage.Data;
using Voyage.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Voyage.IntegrationTests.Data.Extensions
{
    public static class VoyageContextExtensions
    {
        public static ApplicationRole AddRole(this VoyageDataContext context)
        {
            var role = new ApplicationRole { Name = Path.GetRandomFileName() };
            var roleStore = new RoleStore<ApplicationRole>(context);
            roleStore.CreateAsync(role).Wait();
            return role;
        }

        public static RoleClaim AddRoleClaim(this VoyageDataContext context, ApplicationRole role, string type = "type1", string value = "value1")
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
