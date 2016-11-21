namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Add_Deleted_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("core.Users", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("core.Users", "Deleted");
        }
    }
}
