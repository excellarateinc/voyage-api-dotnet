namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleClaims_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleClaims", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.RoleClaims", new[] { "RoleId" });
            DropTable("dbo.RoleClaims");
        }
    }
}
