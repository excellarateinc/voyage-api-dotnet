namespace Launchpad.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewColumnWidgetColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Widgets", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Widgets", "Color");
        }
    }
}
