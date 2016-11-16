using Launchpad.Models;
using Launchpad.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launchpad.Data.Configuration
{
    public class UserPhoneConfiguration : EntityTypeConfiguration<UserPhone>
    {
        public UserPhoneConfiguration()
        {
            this.ToTable("UserPhones", Constants.Schemas.FrameworkTables);
            this.HasKey(_ => _.Id);

            this.Property(_ => _.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            this.Property(_ => _.PhoneNumber);
            this.Property(_ => _.PhoneType);



            HasRequired(_ => _.User)
                .WithMany(_ => _.Phones)
                .HasForeignKey(_ => _.UserId);

        }
    }
}
