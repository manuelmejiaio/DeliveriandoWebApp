namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdenModels", "isSelected", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdenModels", "shortDesc", c => c.String());
            AddColumn("dbo.OrdenModels", "stateHasChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdenModels", "quantity", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenModels", "extPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdenModels", "extPrice");
            DropColumn("dbo.OrdenModels", "quantity");
            DropColumn("dbo.OrdenModels", "stateHasChanged");
            DropColumn("dbo.OrdenModels", "shortDesc");
            DropColumn("dbo.OrdenModels", "isSelected");
        }
    }
}
