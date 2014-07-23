namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Productorders",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        precio = c.Double(nullable: false),
                        isSelected = c.Boolean(nullable: false),
                        shortDesc = c.String(),
                        stateHasChanged = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.OrdenModels", "product_id", c => c.Int());
            AddForeignKey("dbo.OrdenModels", "product_id", "dbo.Productorders", "id");
            CreateIndex("dbo.OrdenModels", "product_id");
            DropColumn("dbo.OrdenModels", "nombre");
            DropColumn("dbo.OrdenModels", "precio");
            DropColumn("dbo.OrdenModels", "isSelected");
            DropColumn("dbo.OrdenModels", "shortDesc");
            DropColumn("dbo.OrdenModels", "stateHasChanged");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdenModels", "stateHasChanged", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdenModels", "shortDesc", c => c.String());
            AddColumn("dbo.OrdenModels", "isSelected", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdenModels", "precio", c => c.Double(nullable: false));
            AddColumn("dbo.OrdenModels", "nombre", c => c.String());
            DropIndex("dbo.OrdenModels", new[] { "product_id" });
            DropForeignKey("dbo.OrdenModels", "product_id", "dbo.Productorders");
            DropColumn("dbo.OrdenModels", "product_id");
            DropTable("dbo.Productorders");
        }
    }
}
