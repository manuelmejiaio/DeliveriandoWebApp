namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lolw : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrdenModels", "product_id", "dbo.ProductOrdens");
            DropIndex("dbo.OrdenModels", new[] { "product_id" });
            AddColumn("dbo.OrdenModels", "nombre", c => c.String());
            AddColumn("dbo.OrdenModels", "precio", c => c.Double(nullable: false));
            AddColumn("dbo.OrdenModels", "isSelected", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdenModels", "shortDesc", c => c.String());
            AddColumn("dbo.OrdenModels", "stateHasChanged", c => c.Boolean(nullable: false));
            DropColumn("dbo.OrdenModels", "product_id");
            DropTable("dbo.ProductOrdens");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductOrdens",
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
            DropColumn("dbo.OrdenModels", "stateHasChanged");
            DropColumn("dbo.OrdenModels", "shortDesc");
            DropColumn("dbo.OrdenModels", "isSelected");
            DropColumn("dbo.OrdenModels", "precio");
            DropColumn("dbo.OrdenModels", "nombre");
            CreateIndex("dbo.OrdenModels", "product_id");
            AddForeignKey("dbo.OrdenModels", "product_id", "dbo.ProductOrdens", "id");
        }
    }
}
