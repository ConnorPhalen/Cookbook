using TechnicalProgrammingProject.Models;

namespace TechnicalProgrammingProject.Migrations
{
    using Microsoft.AspNet.Identity;
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
                    UserName = "BobRoss",
                    Email = "bob@ross.com",
                    PasswordHash = password
                });

            context.Users.ToList().ForEach(u => context.Cookbooks.AddOrUpdate(new Cookbook { ApplicationUserID = u.Id }));

            context.Recipes.AddOrUpdate(r => r.ID,
                                    new Recipe { ID = 1, Name = "Chicken Salad", Description = "Delicious chicken goodness", CookTime = 5, Servings = 6, Directions = "Mix chicken with lettuce", ImageURL = "http://img.sndimg.com/food/image/upload/q_92,fl_progressive/v1/img/recipes/29/89/77/picO9chgt.jpg", Rating = 10 },
                                    new Recipe { ID = 2, Name = "Beef Stroganoff", Description = "Delicious beef goodness", CookTime = 20, Servings = 8, Directions = "Sautee beef", ImageURL = "http://www.gimmesomeoven.com/wp-content/uploads/2014/02/Beef-Stroganoff-1.jpg", Rating = 10 }
            );
            context.SaveChanges();

            context.Ingredients.AddOrUpdate(i => i.ID,
                    new Ingredient { ID = 1, Name = "Chicken", Quantity = 6, Unit = "Oz" },
                    new Ingredient { ID = 2, Name = "Beef", Quantity = 4, Unit = "Lbs" }
            );
            context.SaveChanges();

            context.Users.ToList().ForEach(u => context.Recipes.ToList().ForEach(r => r.ApplicationUser = u));

            var r1 = context.Recipes.Find(1);
            var r2 = context.Recipes.Find(2);

            var i1 = context.Ingredients.Find(1);
            var i2 = context.Ingredients.Find(2);

            r1.Ingredients.Add(i1);
            r2.Ingredients.Add(i2);

            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(r1));
            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(r2));

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
