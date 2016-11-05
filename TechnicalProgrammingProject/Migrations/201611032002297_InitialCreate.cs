namespace TechnicalProgrammingProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cookbooks",
                c => new
                    {
                        CookbookID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CookbookID);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        CookTime = c.Int(nullable: false),
                        Servings = c.Int(nullable: false),
                        ImageURL = c.String(),
                        Directions = c.String(),
                        Rating = c.Int(nullable: false),
                        Cookbook_CookbookID = c.Int(),
                    })
                .PrimaryKey(t => t.RecipeID)
                .ForeignKey("dbo.Cookbooks", t => t.Cookbook_CookbookID)
                .Index(t => t.Cookbook_CookbookID);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientID = c.Int(nullable: false, identity: true),
                        RecipeID = c.Int(nullable: false),
                        Name = c.String(),
                        Quantity = c.Int(nullable: false),
                        Unit = c.String(),
                    })
                .PrimaryKey(t => t.IngredientID)
                .ForeignKey("dbo.Recipes", t => t.RecipeID, cascadeDelete: true)
                .Index(t => t.RecipeID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "Cookbook_CookbookID", "dbo.Cookbooks");
            DropForeignKey("dbo.Ingredients", "RecipeID", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "RecipeID" });
            DropIndex("dbo.Recipes", new[] { "Cookbook_CookbookID" });
            DropTable("dbo.Users");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Recipes");
            DropTable("dbo.Cookbooks");
        }
    }
}
