namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RestauranteModels", "LogoLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RestauranteModels", "LogoLink");
        }
    }
}
