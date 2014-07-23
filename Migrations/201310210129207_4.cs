namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdenModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        IdRestaurante = c.Int(nullable: false),
                        IdPlato = c.Int(nullable: false),
                        CaditadPlato = c.Int(nullable: false),
                        PagoTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrdenModels");
        }
    }
}
