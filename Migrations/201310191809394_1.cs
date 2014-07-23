namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RestauranteModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Rating = c.Int(nullable: false),
                        Ubicacion = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlatoModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdRestaurante = c.Int(nullable: false),
                        Nombre = c.String(),
                        Categoria = c.String(),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PlatoModels");
            DropTable("dbo.RestauranteModels");
        }
    }
}
