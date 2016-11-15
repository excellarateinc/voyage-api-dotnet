using Launchpad.Models.EntityFramework;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Data.Configuration
{
    /*public class IdentityUserConfiguration : EntityTypeConfiguration<IdentityUser>
    {
        public IdentityUserConfiguration()
        {
            ToTable("Users", Constants.Schemas.FrameworkTables);
            HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));
            Property(u => u.Email).HasMaxLength(256);

        }
    }

    public class IdentityUserLoginConfiguration : EntityTypeConfiguration<IdentityUserLogin>
    {
        public IdentityUserLoginConfiguration()
        {
            HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
            ToTable("UserLogins", Constants.Schemas.FrameworkTables);
        }
    }

    public IdentityUserClaimConfiguration : Enttity

    public class IdentityUserRoleConfiguration : EntityTypeConfiguration<IdentityUserRole>
    {
        public IdentityUserRoleConfiguration()
        {
            HasKey(r => new { r.UserId, r.RoleId });
            ToTable("UserRoles", Constants.Schemas.FrameworkTables);
        }
    }*/

    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            this.ToTable("Users", Constants.Schemas.FrameworkTables);
            this.HasKey(_ => _.Id);

            this.Property(_ => _.FirstName).HasMaxLength(128).IsRequired();
            this.Property(_ => _.LastName).HasMaxLength(128).IsRequired();
            this.HasMany(_ => _.Phones);
            this.Property(_ => _.IsActive).IsRequired();
            
        }
    }
}
