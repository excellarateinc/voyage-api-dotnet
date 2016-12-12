using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data.Configuration
{
    public class ApplicationLogConfiguration : EntityTypeConfiguration<ApplicationLog>
    {
        public ApplicationLogConfiguration()
        {
            ToTable("ApplicationLog", Constants.Schemas.FrameworkTables);
            HasKey(_ => _.Id);
            Property(_ => _.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
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
