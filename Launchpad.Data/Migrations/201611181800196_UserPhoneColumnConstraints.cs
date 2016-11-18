namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPhoneColumnConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("core.UserPhones", "PhoneNumber", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("core.UserPhones", "PhoneNumber", c => c.String());
        }
    }
}
