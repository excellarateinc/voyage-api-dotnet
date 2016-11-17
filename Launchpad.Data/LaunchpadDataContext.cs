using Launchpad.Models.EntityFramework;
using Launchpad.Data.Interfaces;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Launchpad.Data
{


    public class LaunchpadDataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, ILaunchpadDataContext
    {

        #region DbSets

        public IDbSet<Widget> Widgets { get; set; }
        public IDbSet<LaunchpadLog> Logs {get;set;}

        public IDbSet<RoleClaim> RoleClaims { get; set; }

        #endregion 


        public LaunchpadDataContext() : base("LaunchpadDataContext")
        {

        }

        /// <summary>
        /// Pass in the connection string to eliminate the "magic string" above
        /// </summary>
        /// <param name="connectionString">Connection string from the web.config or app.config</param>
        public LaunchpadDataContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            #region Boilerplate configuration
            //Migrations were not being generated corretly because the order in which the base was executing
            //the model configurations and then the attempt to rename the tables. As a result, easiest solution was to take the base code
            //move it here and make it explicit
            //https://aspnetidentity.codeplex.com/SourceControl/latest#src/Microsoft.AspNet.Identity.EntityFramework/IdentityDbContext.cs
            var user = modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users", Constants.Schemas.FrameworkTables);
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));

            // CONSIDER: u.Email is Required if set on options?
            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("UserRoles", Constants.Schemas.FrameworkTables);

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("UserLogins", Constants.Schemas.FrameworkTables);

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("UserClaims", Constants.Schemas.FrameworkTables);

            var role = modelBuilder.Entity<ApplicationRole>()
                .ToTable("Roles", Constants.Schemas.FrameworkTables);

            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
            #endregion

            //Register the other models
            modelBuilder.Configurations.AddFromAssembly(typeof(LaunchpadDataContext).Assembly);

        }
    }
}
