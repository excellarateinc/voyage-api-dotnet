namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTableLaunchpadLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LaunchpadLogs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Message = c.String(),
                    MessageTemplate = c.String(),
                    Level = c.String(),
                    TimeStamp = c.DateTime(nullable: false),
                    Exception = c.String(),
                    LogEvent = c.String(),
                    Properties = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.LaunchpadLogs");
        }
    }
}
