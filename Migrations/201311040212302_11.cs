namespace DeliveriandoWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Model_Id", "dbo.Models");
            DropIndex("dbo.Products", new[] { "Model_Id" });
            DropColumn("dbo.Products", "Model_Id");
            DropTable("dbo.Models");
        }
        
        public override void Down()
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
            CreateIndex("dbo.Products", "Model_Id");
            AddForeignKey("dbo.Products", "Model_Id", "dbo.Models", "Id");
        }
    }
}
