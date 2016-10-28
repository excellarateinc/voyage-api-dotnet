using Launchpad.Models.EntityFramework;
using System.Data.Entity.ModelConfiguration;

namespace Launchpad.Data.Configuration
{
    public class RoleClaimsConfiguration : EntityTypeConfiguration<RoleClaim>
    {
        public RoleClaimsConfiguration()
        {
            ToTable("RoleClaims");

            HasKey(_ => _.Id);

            Property(_ => _.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(_ => _.ClaimType);
            Property(_ => _.ClaimValue);

            Property(_ => _.RoleId)
                .IsRequired();

            HasRequired(_ => _.Role)
                .WithMany(_ => _.Claims)
                .HasForeignKey(_ => _.RoleId);
                             

        }
    }
}
