using Launchpad.Models.EntityFramework;
using System.Data.Entity.ModelConfiguration;

namespace Launchpad.Data.Configuration
{

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
