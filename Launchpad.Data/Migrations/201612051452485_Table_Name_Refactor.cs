namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table_Name_Refactor : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "core.LaunchpadLogs", newName: "ApplicationLog");
            RenameTable(name: "core.RoleClaims", newName: "RoleClaim");
            RenameTable(name: "core.Roles", newName: "Role");
            RenameTable(name: "core.UserRoles", newName: "UserRole");
            RenameTable(name: "core.UserPhones", newName: "UserPhone");
            RenameTable(name: "core.Users", newName: "User");
            RenameTable(name: "core.UserClaims", newName: "UserClaim");
            RenameTable(name: "core.UserLogins", newName: "UserLogin");
            MoveTable(name: "core.ActivityAudit", newSchema: "dbo");
            MoveTable(name: "core.AuditLog", newSchema: "dbo");
            MoveTable(name: "core.AuditLogDetail", newSchema: "dbo");
            MoveTable(name: "core.LogMetadata", newSchema: "dbo");
            MoveTable(name: "core.ApplicationLog", newSchema: "dbo");
            MoveTable(name: "core.RoleClaim", newSchema: "dbo");
            MoveTable(name: "core.Role", newSchema: "dbo");
            MoveTable(name: "core.UserRole", newSchema: "dbo");
            MoveTable(name: "core.UserPhone", newSchema: "dbo");
            MoveTable(name: "core.User", newSchema: "dbo");
            MoveTable(name: "core.UserClaim", newSchema: "dbo");
            MoveTable(name: "core.UserLogin", newSchema: "dbo");
            MoveTable(name: "core.Widget", newSchema: "dbo");
        }
        
        public override void Down()
        {
            MoveTable(name: "dbo.Widget", newSchema: "core");
            MoveTable(name: "dbo.UserLogin", newSchema: "core");
            MoveTable(name: "dbo.UserClaim", newSchema: "core");
            MoveTable(name: "dbo.User", newSchema: "core");
            MoveTable(name: "dbo.UserPhone", newSchema: "core");
            MoveTable(name: "dbo.UserRole", newSchema: "core");
            MoveTable(name: "dbo.Role", newSchema: "core");
            MoveTable(name: "dbo.RoleClaim", newSchema: "core");
            MoveTable(name: "dbo.ApplicationLog", newSchema: "core");
            MoveTable(name: "dbo.LogMetadata", newSchema: "core");
            MoveTable(name: "dbo.AuditLogDetail", newSchema: "core");
            MoveTable(name: "dbo.AuditLog", newSchema: "core");
            MoveTable(name: "dbo.ActivityAudit", newSchema: "core");
            RenameTable(name: "core.UserLogin", newName: "UserLogins");
            RenameTable(name: "core.UserClaim", newName: "UserClaims");
            RenameTable(name: "core.User", newName: "Users");
            RenameTable(name: "core.UserPhone", newName: "UserPhones");
            RenameTable(name: "core.UserRole", newName: "UserRoles");
            RenameTable(name: "core.Role", newName: "Roles");
            RenameTable(name: "core.RoleClaim", newName: "RoleClaims");
            RenameTable(name: "core.ApplicationLog", newName: "LaunchpadLogs");
        }
    }
}
