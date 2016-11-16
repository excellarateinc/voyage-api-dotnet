namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUserPhonesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "core.UserPhones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        PhoneNumber = c.String(),
                        PhoneType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("core.UserPhones", "UserId", "core.Users");
            DropIndex("core.UserPhones", new[] { "UserId" });
            DropTable("core.UserPhones");
        }
    }
}
