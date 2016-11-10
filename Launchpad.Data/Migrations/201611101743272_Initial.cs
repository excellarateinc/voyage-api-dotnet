namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "core.LaunchpadLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        MessageTemplate = c.String(),
                        Level = c.String(maxLength: 128),
                        TimeStamp = c.DateTime(nullable: false),
                        Exception = c.String(),
                        LogEvent = c.String(storeType: "xml"),
                        Properties = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "core.RoleClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "core.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "core.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("core.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("core.Users", t => t.ApplicationUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "core.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 128),
                        LastName = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "core.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.Users", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "core.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("core.Users", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "core.Widget",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 60),
                        Color = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("core.UserRoles", "ApplicationUser_Id", "core.Users");
            DropForeignKey("core.UserLogins", "ApplicationUser_Id", "core.Users");
            DropForeignKey("core.UserClaims", "ApplicationUser_Id", "core.Users");
            DropForeignKey("core.RoleClaims", "RoleId", "core.Roles");
            DropForeignKey("core.UserRoles", "RoleId", "core.Roles");
            DropIndex("core.UserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("core.UserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("core.UserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("core.UserRoles", new[] { "RoleId" });
            DropIndex("core.Roles", "RoleNameIndex");
            DropIndex("core.RoleClaims", new[] { "RoleId" });
            DropTable("core.Widget");
            DropTable("core.UserLogins");
            DropTable("core.UserClaims");
            DropTable("core.Users");
            DropTable("core.UserRoles");
            DropTable("core.Roles");
            DropTable("core.RoleClaims");
            DropTable("core.LaunchpadLogs");
        }
    }
}
