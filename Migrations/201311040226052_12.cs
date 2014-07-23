namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IdRestaurante", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Categoria", c => c.String());
            AddColumn("dbo.Products", "Descripcion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Descripcion");
            DropColumn("dbo.Products", "Categoria");
            DropColumn("dbo.Products", "IdRestaurante");
        }
    }
}
