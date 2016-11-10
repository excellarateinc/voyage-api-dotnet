namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LaunchpadLog_Fix_ColumnTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("core.LaunchpadLogs", "LogEvent", c => c.String());
            AlterColumn("core.LaunchpadLogs", "Properties", c => c.String(storeType: "xml"));
        }
        
        public override void Down()
        {
            AlterColumn("core.LaunchpadLogs", "Properties", c => c.String());
            AlterColumn("core.LaunchpadLogs", "LogEvent", c => c.String(storeType: "xml"));
        }
    }
}
