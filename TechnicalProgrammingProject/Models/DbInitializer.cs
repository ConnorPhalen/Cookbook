using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TechnicalProgrammingProject.Models
{
    public class DbInitializer : DropCreateDatabaseAlways<RepositoryDbContext>
    {
        protected override void Seed(RepositoryDbContext context)
        {
            var user = new List<User>
                {
                    new User { Name = "Admin"},
                    new User { Name = "Hulk Hogan" }
                };

            user.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            context.Users.ToList<User>().ForEach(u => context.Cookbooks.Add(new Cookbook { UserID = u.UserID }));
            context.SaveChanges();

            var recipe = new List<Recipe>
                {
                    new Recipe { UserID = context.Users.Find(1).UserID, Name = "Chicken Salad", Description = "Delicious chicken goodness", CookTime = 5, Servings = 6, Directions = "Mix chicken with lettuce", ImageURL = "http://img.sndimg.com/food/image/upload/q_92,fl_progressive/v1/img/recipes/29/89/77/picO9chgt.jpg", Rating = 10},
                    new Recipe { UserID = context.Users.Find(2).UserID, Name = "Beef Stroganoff", Description = "Delicious beef goodness", CookTime = 20, Servings = 8, Directions = "Sautee beef", ImageURL = "http://www.gimmesomeoven.com/wp-content/uploads/2014/02/Beef-Stroganoff-1.jpg", Rating = 10}
                };

            recipe.ForEach(r => context.Recipes.Add(r));
            context.SaveChanges();

            var ingredient = new List<Ingredient>
                {
                    new Ingredient { RecipeID = context.Recipes.Find(1).RecipeID, Name = "Chicken", Quantity=6, Unit = "Oz" },
                    new Ingredient { RecipeID = context.Recipes.Find(2).RecipeID, Name = "Beef", Quantity=4, Unit = "Lbs" }
                };

            ingredient.ForEach(i => context.Ingredients.Add(i));
            context.SaveChanges();
        }
    }
}