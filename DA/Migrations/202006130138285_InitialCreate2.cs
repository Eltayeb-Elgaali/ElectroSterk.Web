namespace DA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Reviews", "OrderUserId", "dbo.OrderUsers");
            DropIndex("dbo.Reviews", new[] { "ProductId" });
            DropIndex("dbo.Reviews", new[] { "OrderUserId" });
            AlterColumn("dbo.Reviews", "Rating", c => c.Double());
            AlterColumn("dbo.Reviews", "PostDate", c => c.DateTime());
            AlterColumn("dbo.Reviews", "ProductId", c => c.Int());
            AlterColumn("dbo.Reviews", "OrderUserId", c => c.Int());
            CreateIndex("dbo.Reviews", "ProductId");
            CreateIndex("dbo.Reviews", "OrderUserId");
            AddForeignKey("dbo.Reviews", "ProductId", "dbo.Products", "Id");
            AddForeignKey("dbo.Reviews", "OrderUserId", "dbo.OrderUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "OrderUserId", "dbo.OrderUsers");
            DropForeignKey("dbo.Reviews", "ProductId", "dbo.Products");
            DropIndex("dbo.Reviews", new[] { "OrderUserId" });
            DropIndex("dbo.Reviews", new[] { "ProductId" });
            AlterColumn("dbo.Reviews", "OrderUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reviews", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reviews", "PostDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Reviews", "Rating", c => c.Double(nullable: false));
            CreateIndex("dbo.Reviews", "OrderUserId");
            CreateIndex("dbo.Reviews", "ProductId");
            AddForeignKey("dbo.Reviews", "OrderUserId", "dbo.OrderUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reviews", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
