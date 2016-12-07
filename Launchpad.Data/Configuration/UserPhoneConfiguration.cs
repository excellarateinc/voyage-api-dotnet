using Launchpad.Models.EntityFramework;
using System.Data.Entity.ModelConfiguration;

namespace Launchpad.Data.Configuration
{
    public class UserPhoneConfiguration : EntityTypeConfiguration<UserPhone>
    {
        public UserPhoneConfiguration()
        {
            this.ToTable("UserPhone", Constants.Schemas.FrameworkTables);
            this.HasKey(_ => _.Id);

            this.Property(_ => _.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            this.Property(_ => _.PhoneNumber).HasMaxLength(15).IsRequired();
            this.Property(_ => _.PhoneType).IsRequired();

            HasRequired(_ => _.User)
                .WithMany(_ => _.Phones)
                .HasForeignKey(_ => _.UserId);

        }
    }
}
