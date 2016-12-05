using Launchpad.Models.EntityFramework;
using System.Data.Entity.ModelConfiguration;

namespace Launchpad.Data.Configuration
{
    public class LaunchpadLogConfiguration : EntityTypeConfiguration<LaunchpadLog>
    {
        public LaunchpadLogConfiguration()
        {
            ToTable("ApplicationLog", Constants.Schemas.FrameworkTables);

            HasKey(_ => _.Id);

            Property(_ => _.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(_ => _.Message);
            Property(_ => _.MessageTemplate);
            Property(_ => _.Level).HasMaxLength(128);
            Property(_ => _.TimeStamp);
            Property(_ => _.Exception);
            Property(_ => _.Properties).HasColumnType("xml");
            Property(_ => _.LogEvent);
        }
    }
}
