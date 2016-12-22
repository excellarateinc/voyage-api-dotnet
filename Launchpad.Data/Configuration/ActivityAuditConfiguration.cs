using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Launchpad.Models.EntityFramework;

namespace Launchpad.Data.Configuration
{
    public class ActivityAuditConfiguration : EntityTypeConfiguration<ActivityAudit>
    {
        public ActivityAuditConfiguration()
        {                          
            ToTable("ActivityAudit");
            HasKey(_ => _.Id);
            Property(_ => _.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(_ => _.RequestId).HasMaxLength(64);
            Property(_ => _.Method).HasMaxLength(32);
            Property(_ => _.Path).HasMaxLength(128);
            Property(_ => _.IpAddress).HasMaxLength(64);
            Property(_ => _.Date);
            Property(_ => _.StatusCode);
            Property(_ => _.Error);
            Property(_ => _.UserName).HasMaxLength(50);   
        }
    }
}
