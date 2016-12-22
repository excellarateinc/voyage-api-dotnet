using System.Data.Entity.ModelConfiguration;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data.Configuration
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            ToTable("User");
            HasKey(_ => _.Id);
            Property(_ => _.FirstName).HasMaxLength(128).IsRequired();
            Property(_ => _.LastName).HasMaxLength(128).IsRequired();
            HasMany(_ => _.Phones);
            Property(_ => _.IsActive).IsRequired();
            Property(_ => _.Deleted).IsRequired();
        }
    }
}
