using Launchpad.Models.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Launchpad.Data.Configuration
{
    public class WidgetConfiguration : EntityTypeConfiguration<Widget>
    {
        public WidgetConfiguration()
        {
            HasKey(_ => _.Id);
            Property(_ => _.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(_ => _.Name).HasMaxLength(60);

        }
    }
}
