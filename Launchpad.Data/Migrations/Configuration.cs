namespace Launchpad.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Launchpad.Data.LaunchpadDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Useful for getting access to EntityValdiationErrors
        /// </summary>
        /// <param name="context"></param>
        /// <remarks>Reference: http://www.blaiseliu.com/got-entityvalidationerrors-debug-into-entity-framework-code-first </remarks>
        private static void SaveChanges(LaunchpadDataContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
        }

        protected override void Seed(Launchpad.Data.LaunchpadDataContext context)
        {
            const string claimType = "lss.permission";

            //Let's seed the database with a user, some roles, and permissions

            #region User Seed

            var adminUser = new Models.EntityFramework.ApplicationUser()
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                PasswordHash = new PasswordHasher().HashPassword("Hello123!"),
                SecurityStamp = Guid.NewGuid().ToString()
            };
          
            context.Users.AddOrUpdate(_ => _.UserName, adminUser);

            SaveChanges(context);
            #endregion


            #region Role Seed

            var basicRole = new Models.EntityFramework.ApplicationRole
            {
                Name = "Basic",
                Id = Guid.NewGuid().ToString()
            };
            var adminRole = new Models.EntityFramework.ApplicationRole
            {
                Name = "Administrator",
                Id = Guid.NewGuid().ToString()
            };
            
            context.Roles.AddOrUpdate(_ => _.Name, basicRole, adminRole);

            SaveChanges(context);
            #endregion

            #region User-Role assignment
            
            AddRole(adminUser, basicRole.Id);
            AddRole(adminUser, adminRole.Id);

            context.Users.AddOrUpdate(adminUser);

            SaveChanges(context);
            #endregion

            #region Claim-Role assignment

            SeedRoleClaim(context, basicRole.Id, claimType, "login");
            SeedRoleClaim(context, basicRole.Id, claimType, "list.user-claims");


            SeedRoleClaim(context, adminRole.Id, claimType, "assign.role");
            SeedRoleClaim(context, adminRole.Id, claimType, "create.role");
            SeedRoleClaim(context, adminRole.Id, claimType, "delete.role");
            SeedRoleClaim(context, adminRole.Id, claimType, "list.roles");
            SeedRoleClaim(context, adminRole.Id, claimType, "revoke.role");

            SeedRoleClaim(context, adminRole.Id, claimType, "view.claim");

            SeedRoleClaim(context, adminRole.Id, claimType, "list.users");
            SeedRoleClaim(context, adminRole.Id, claimType, "list.user-claims");
            SeedRoleClaim(context, adminRole.Id, claimType, "view.user");
            SeedRoleClaim(context, adminRole.Id, claimType, "update.user");
            SeedRoleClaim(context, adminRole.Id, claimType, "delete.user");


            SeedRoleClaim(context, adminRole.Id, claimType, "delete.role-claim");
            SeedRoleClaim(context, adminRole.Id, claimType, "create.claim");
            
            SaveChanges(context);
            #endregion

            #region Widget

            context.Widgets.AddOrUpdate(_ => _.Name, new Widget
            {
                Name = "Seed Widget",
                Color = "Green"
            });

            SaveChanges(context);
            #endregion 

        }

        private void AddRole(ApplicationUser user, string roleId)
        {
            if (!user.Roles.Any(_ => _.RoleId == roleId))
                user.Roles.Add(new IdentityUserRole() { RoleId = roleId, UserId = user.Id });
        }

        private void SeedRoleClaim(LaunchpadDataContext context, string roleId, string claimType, string claimValue)
        {
            if (!context.RoleClaims.Any(_ => _.RoleId == roleId && _.ClaimType == claimType && _.ClaimValue == claimValue))
                context.RoleClaims.AddOrUpdate(
                new Models.EntityFramework.RoleClaim
                {
                    ClaimType = claimType,
                    ClaimValue = claimValue,
                    RoleId = roleId
                });

        }
    }
}
