namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable_Ef_Audit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "core.AuditLog",
                c => new
                    {
                        AuditLogId = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        EventDateUTC = c.DateTime(nullable: false),
                        EventType = c.Int(nullable: false),
                        TypeFullName = c.String(nullable: false, maxLength: 512),
                        RecordId = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.AuditLogId);
            
            CreateTable(
                "core.AuditLogDetail",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PropertyName = c.String(nullable: false, maxLength: 256),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                        AuditLogId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.AuditLog", t => t.AuditLogId, cascadeDelete: true)
                .Index(t => t.AuditLogId);
            
            CreateTable(
                "core.LogMetadata",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AuditLogId = c.Long(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.AuditLog", t => t.AuditLogId, cascadeDelete: true)
                .Index(t => t.AuditLogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("core.LogMetadata", "AuditLogId", "core.AuditLog");
            DropForeignKey("core.AuditLogDetail", "AuditLogId", "core.AuditLog");
            DropIndex("core.LogMetadata", new[] { "AuditLogId" });
            DropIndex("core.AuditLogDetail", new[] { "AuditLogId" });
            DropTable("core.LogMetadata");
            DropTable("core.AuditLogDetail");
            DropTable("core.AuditLog");
        }
    }
}
