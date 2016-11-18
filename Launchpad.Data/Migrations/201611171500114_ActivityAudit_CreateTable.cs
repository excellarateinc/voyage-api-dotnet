namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityAudit_CreateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "core.ActivityAudit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.String(maxLength: 64),
                        Method = c.String(maxLength: 32),
                        Path = c.String(maxLength: 128),
                        IpAddress = c.String(maxLength: 64),
                        Date = c.DateTime(nullable: false),
                        StatusCode = c.Int(nullable: false),
                        Error = c.String(),
                        UserName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("core.ActivityAudit");
        }
    }
}
