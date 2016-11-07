using TechnicalProgrammingProject.Models;

namespace TechnicalProgrammingProject.Migrations
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TechnicalProgrammingProject.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var passwordHasher = new PasswordHasher();
            string password = passwordHasher.HashPassword("Cooking1!");
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = "bob@ross.com",
                    Email = "bob@ross.com",
                    PasswordHash = password,
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.SaveChanges();

            context.Users.ToList().ForEach(u => context.Cookbooks.AddOrUpdate(c => c.ApplicationUserID, new Cookbook { ApplicationUser = u }));

            context.SaveChanges();

            List<Recipe> recipes = new List<Recipe>()
            {
                new Recipe
                {
                    ID = 1,
                    Name = "Bacon",
                    Description = "Savoury Grease and Meat make the perfecxt combo.",
                    CookTime = 4,
                    Servings = 1,
                    Directions = "Step 1.) Get Bacon \n Step 2.) Cook Bacon \n Step 3.) Eat Bacon",
                    ImageURL = "http://img.webmd.com/dtmcms/live/webmd/consumer_assets/site_images/article_thumbnails/quizzes/bacon_sizzling_facts_rmq/650x350_bacon_sizzling_facts_rmq.jpg",
                    Rating = 11
                },
                 new Recipe
                {
                    ID = 2,
                    Name = "Mexican Taco",
                    Description = "The legit taco of the Mexican people.",
                    CookTime = 12,
                    Servings = 4,
                    Directions = "Taco Time!",
                    ImageURL = "http://eatneat.com/wp-content/themes/neat/images/tacos.png",
                    Rating = 10
                },
                  new Recipe
                {
                    ID = 3,
                    Name = "Belgian Waffle",
                    Description = "The perfect waffle.",
                    CookTime = 0,
                    Servings = 1,
                    Directions = "Just go to Belgium and buy it. You cannot make it as good as they can.",
                    ImageURL = "http://cdn1.tmbi.com/TOH/Images/Photos/37/300x300/exps4869_RDS2928497A10_11_1b_WEB.jpg",
                    Rating = 69
                },
                   new Recipe
                {
                    ID = 4,
                    Name = "Chicken Salad",
                    Description = "Delicious chicken goodness",
                    CookTime = 5,
                    Servings = 6,
                    Directions = "Mix chicken with lettuce",
                    ImageURL = "http://img.sndimg.com/food/image/upload/q_92,fl_progressive/v1/img/recipes/29/89/77/picO9chgt.jpg",
                    Rating = 9
                },
                new Recipe
                {
                    ID = 5,
                    Name = "Beef Stroganoff",
                    Description = "Delicious beef goodness",
                    CookTime = 20,
                    Servings = 8,
                    Directions = "Sautee beef",
                    ImageURL = "http://www.gimmesomeoven.com/wp-content/uploads/2014/02/Beef-Stroganoff-1.jpg",
                    Rating = 8
                }
            };

            recipes.ForEach(r => context.Recipes.AddOrUpdate(rc => rc.ID, r));

            context.SaveChanges();

            List<Ingredient> ingredients = new List<Ingredient>()
            {
                new Ingredient { ID = 1, Name = "Chicken", Quantity = 6, Unit = "Oz" },
                new Ingredient { ID = 2, Name = "Beef", Quantity = 4, Unit = "Lbs" }
            };

            ingredients.ForEach(i => context.Ingredients.AddOrUpdate(ig => ig.ID, i));

            context.SaveChanges();

            context.Users.ToList().ForEach(u => context.Recipes.ToList().ForEach(r => r.ApplicationUser = u));

            var recipe1 = context.Recipes.Find(1);
            var recipe2 = context.Recipes.Find(2);

            var ingredient1 = context.Ingredients.Find(1);
            var ingredient2 = context.Ingredients.Find(2);

            recipe1.Ingredients.Add(ingredient1);
            recipe2.Ingredients.Add(ingredient2);

            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(recipe1));
            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(recipe2));

            context.SaveChanges();
        }
    }
}
