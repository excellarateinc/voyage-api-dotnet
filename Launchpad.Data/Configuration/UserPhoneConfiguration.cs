using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data.Configuration
{
    public class UserPhoneConfiguration : EntityTypeConfiguration<UserPhone>
    {
        public UserPhoneConfiguration()
        {
            ToTable("UserPhone", Constants.Schemas.FrameworkTables);
            HasKey(_ => _.Id);

            Property(_ => _.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(_ => _.PhoneNumber).HasMaxLength(15).IsRequired();
            Property(_ => _.PhoneType).IsRequired();

            HasRequired(_ => _.User)
                .WithMany(_ => _.Phones)
                .HasForeignKey(_ => _.UserId);
        }
    }
}
