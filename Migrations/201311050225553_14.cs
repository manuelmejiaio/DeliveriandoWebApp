namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdenModels", "nombre", c => c.String());
            AddColumn("dbo.OrdenModels", "precio", c => c.Double(nullable: false));
            AddColumn("dbo.OrdenModels", "isSelected", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdenModels", "shortDesc", c => c.String());
            AddColumn("dbo.OrdenModels", "stateHasChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdenModels", "quantity", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenModels", "extPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.OrdenModels", "id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.OrdenModels", new[] { "ID" });
            AddPrimaryKey("dbo.OrdenModels", "id");
            DropColumn("dbo.OrdenModels", "IdCliente");
            DropColumn("dbo.OrdenModels", "IdRestaurante");
            DropColumn("dbo.OrdenModels", "IdPlato");
            DropColumn("dbo.OrdenModels", "CaditadPlato");
            DropColumn("dbo.OrdenModels", "PagoTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdenModels", "PagoTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrdenModels", "CaditadPlato", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenModels", "IdPlato", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenModels", "IdRestaurante", c => c.Int(nullable: false));
            AddColumn("dbo.OrdenModels", "IdCliente", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.OrdenModels", new[] { "id" });
            AddPrimaryKey("dbo.OrdenModels", "ID");
            AlterColumn("dbo.OrdenModels", "ID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.OrdenModels", "extPrice");
            DropColumn("dbo.OrdenModels", "quantity");
            DropColumn("dbo.OrdenModels", "stateHasChanged");
            DropColumn("dbo.OrdenModels", "shortDesc");
            DropColumn("dbo.OrdenModels", "isSelected");
            DropColumn("dbo.OrdenModels", "precio");
            DropColumn("dbo.OrdenModels", "nombre");
        }
    }
}
