using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data.Configuration
{
    public class RoleClaimsConfiguration : EntityTypeConfiguration<RoleClaim>
    {
        public RoleClaimsConfiguration()
        {
            ToTable("RoleClaim", Constants.Schemas.FrameworkTables);

            HasKey(_ => _.Id);

            Property(_ => _.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

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
