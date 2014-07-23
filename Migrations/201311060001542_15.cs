namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrdenModels", "isSelected");
            DropColumn("dbo.OrdenModels", "shortDesc");
            DropColumn("dbo.OrdenModels", "stateHasChanged");
            DropColumn("dbo.OrdenModels", "quantity");
            DropColumn("dbo.OrdenModels", "extPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdenModels", "extPrice", c => c.Double(nullable: false));
            AddColumn("dbo.OrdenModels", "quantity", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenModels", "stateHasChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdenModels", "shortDesc", c => c.String());
            AddColumn("dbo.OrdenModels", "isSelected", c => c.Boolean(nullable: false));
        }
    }
}
