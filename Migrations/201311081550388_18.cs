namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdenModels", "grandTotal", c => c.Double(nullable: false));
            DropColumn("dbo.OrdenModels", "nombre");
            DropColumn("dbo.OrdenModels", "precio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdenModels", "precio", c => c.Double(nullable: false));
            AddColumn("dbo.OrdenModels", "nombre", c => c.String());
            DropColumn("dbo.OrdenModels", "grandTotal");
        }
    }
}
