namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Models",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Brand = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "Model_Id", c => c.Long());
            AddForeignKey("dbo.Products", "Model_Id", "dbo.Models", "Id");
            CreateIndex("dbo.Products", "Model_Id");
            DropColumn("dbo.Products", "Brand");
            DropColumn("dbo.Products", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Name", c => c.String());
            AddColumn("dbo.Products", "Brand", c => c.String());
            DropIndex("dbo.Products", new[] { "Model_Id" });
            DropForeignKey("dbo.Products", "Model_Id", "dbo.Models");
            DropColumn("dbo.Products", "Model_Id");
            DropTable("dbo.Models");
        }
    }
}
