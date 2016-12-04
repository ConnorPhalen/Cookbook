using TechnicalProgrammingProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Reflection;
using System;

namespace TechnicalProgrammingProject.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TechnicalProgrammingProject.Models.ApplicationDbContext";

        }

        protected override void Seed(ApplicationDbContext context)
        {
            // ---- New Code to Create Roles ----
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // first we create SuperAdmin role   
            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            role.Name = "SuperAdmin";
            roleManager.Create(role);

            // first we create SuperAdmin role   
            var role1 = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            role1.Name = "Moderator";
            roleManager.Create(role1);

            // first we create SuperAdmin role   
            var role2 = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            role2.Name = "User";
            roleManager.Create(role2);

            // ---- End New Code to Create Roles ----

            /*
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
            */

            // ---- New Code to Create Users ----

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            var avatarPath = Path.Combine(directoryName, ".." + "~/Content/asset/images/avatar.jpg".TrimStart('~').Replace('/', '\\'));
            var realAvatarPath = Path.GetFullPath(avatarPath);
            Image avatarImage = Image.FromFile(realAvatarPath);
            byte[] avatarByte;
            using (MemoryStream ms = new MemoryStream())
            {
                avatarImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                avatarByte = ms.ToArray();
            }

            var user = new ApplicationUser();
            user.UserName = "HulkHogan";
            user.Email = "HulkHogan@WWF.com";
            user.DisplayName = "Hulkamania";

            user.ProfileImage = avatarByte;
            string userPWD = "HulkHogan@42";

            var chkUser = UserManager.Create(user, userPWD);

            //Add default User to Role SuperAdmin   
            if (chkUser.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, "SuperAdmin");
            }

            //Here we create a Moderator user who will moderate the website 
            var user1 = new ApplicationUser();
            user1.UserName = "BobRoss";
            user1.Email = "bob@ross.com";
            user1.DisplayName = "The Joy of Painting";
            user1.ProfileImage = avatarByte;

            string userPWD1 = "Cooking1!";

            var chkUser1 = UserManager.Create(user1, userPWD1);

            //Add default User to Role Moderator   
            if (chkUser1.Succeeded)
            {
                var result1 = UserManager.AddToRole(user1.Id, "Moderator");
            }

            //Here we create a User who will uplaod some recipes
            var user2 = new ApplicationUser();
            user2.UserName = "Weeb";
            user2.Email = "Weeb@anime.com";
            user2.DisplayName = "So Kawaii!";
            user2.ProfileImage = avatarByte;

            string userPWD2 = "ilovebodypillows@42";

            var chkUser2 = UserManager.Create(user2, userPWD2);

            //Add default User to Role User   
            if (chkUser2.Succeeded)
            {
                var result2 = UserManager.AddToRole(user2.Id, "User");
            }

            // ---- End New Code to Create Users ---- //

            context.SaveChanges();

            context.Users.ToList().ForEach(u => context.Cookbooks.AddOrUpdate(c => c.ApplicationUserID, new Cookbook { ApplicationUser = u }));

            context.SaveChanges();

            var beefPath = Path.Combine(directoryName, ".." + "~/Content/Images/beef.jpg".TrimStart('~').Replace('/', '\\'));
            var realBeefPath = Path.GetFullPath(beefPath);
            Image beefImage = Image.FromFile(realBeefPath);
            byte[] beefByte;
            using (MemoryStream ms = new MemoryStream())
            {
                beefImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                beefByte = ms.ToArray();
            }

            var wafflePath = Path.Combine(directoryName, ".." + "~/Content/Images/waffle.jpg".TrimStart('~').Replace('/', '\\'));
            var realWafflePath = Path.GetFullPath(wafflePath);
            Image waffleImage = Image.FromFile(realWafflePath);
            byte[] waffleByte;
            using (MemoryStream ms = new MemoryStream())
            {
                waffleImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                waffleByte = ms.ToArray();
            }

            var chickenPath = Path.Combine(directoryName, ".." + "~/Content/Images/chicken.jpg".TrimStart('~').Replace('/', '\\'));
            var realChickenPath = Path.GetFullPath(chickenPath);
            Image chickenImage = Image.FromFile(realChickenPath);
            byte[] chickenByte;
            using (MemoryStream ms = new MemoryStream())
            {
                chickenImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                chickenByte = ms.ToArray();
            }

            var tacoPath = Path.Combine(directoryName, ".." + "~/Content/Images/taco.png".TrimStart('~').Replace('/', '\\'));
            var realTacoPath = Path.GetFullPath(tacoPath);
            Image tacoImage = Image.FromFile(realTacoPath);
            byte[] tacoByte;
            using (MemoryStream ms = new MemoryStream())
            {
                tacoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                tacoByte = ms.ToArray();
            }

            var baconPath = Path.Combine(directoryName, ".." + "~/Content/Images/bacon.jpg".TrimStart('~').Replace('/', '\\'));
            var realBaconPath = Path.GetFullPath(baconPath);
            Image baconImage = Image.FromFile(realBaconPath);
            byte[] baconByte;
            using (MemoryStream ms = new MemoryStream())
            {
                baconImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                baconByte = ms.ToArray();
            }

            List<Recipe> recipes = new List<Recipe>()
            {
                new Recipe
                {
                    ID = 1,
                    Name = "Bacon",
                    Description = "Savoury Grease and Meat make the perfect combo.",
                    CookTime = 4,
                    Servings = 1,
                    Directions = "Step 1.) Get Bacon \n Step 2.) Cook Bacon \n Step 3.) Eat Bacon",
                    ImageURL = baconByte
                },
                 new Recipe
                {
                    ID = 2,
                    Name = "Mexican Taco",
                    Description = "The legit taco of the Mexican people.",
                    CookTime = 12,
                    Servings = 4,
                    Directions = "Taco Time!",
                    ImageURL = tacoByte,
                },
                  new Recipe
                {
                    ID = 3,
                    Name = "Belgian Waffle",
                    Description = "The perfect waffle.",
                    CookTime = 0,
                    Servings = 1,
                    Directions = "Just go to Belgium and buy it. You cannot make it as good as they can.",
                    ImageURL = waffleByte
                },
                   new Recipe
                {
                    ID = 4,
                    Name = "Chicken Salad",
                    Description = "Delicious chicken goodness",
                    CookTime = 5,
                    Servings = 6,
                    Directions = "Mix chicken with lettuce",
                    ImageURL = chickenByte
                },
                new Recipe
                {
                    ID = 5,
                    Name = "Beef Stroganoff",
                    Description = "Delicious beef goodness",
                    CookTime = 20,
                    Servings = 8,
                    Directions = "Sautee beef",
                    ImageURL = beefByte
                }
            };

            recipes.ForEach(r => context.Recipes.AddOrUpdate(rc => rc.ID, r));

            context.SaveChanges();

            List<Ingredient> ingredients = new List<Ingredient>()
            {
                new Ingredient { ID = 1, Name = "Bacon", Quantity = 20, Unit = "Grams" },
                new Ingredient { ID = 2, Name = "Soft Taco Shell", Quantity = 2, Unit = "" },
                new Ingredient { ID = 3, Name = "Chicken", Quantity = 6, Unit = "KiloGrams" },
                new Ingredient { ID = 4, Name = "Beef", Quantity = 4, Unit = "KiloGrams" },
                new Ingredient { ID = 5, Name = "Belgian Waffle", Quantity = 1, Unit = "Grams" },
                new Ingredient { ID = 6, Name = "Strawberries", Quantity = 6, Unit = "Grams" },
                new Ingredient { ID = 7, Name = "Salad", Quantity = 1, Unit = "KiloGrams" }
            };

            ingredients.ForEach(i => context.Ingredients.AddOrUpdate(ig => ig.ID, i));

            context.SaveChanges();

            List<Tag> tags = new List<Tag>()
            {
                new Tag { ID = 1, Name = "Beef" },
                new Tag { ID = 2, Name = "Chicken" },
                new Tag { ID = 3, Name = "Healthy" },
                new Tag { ID = 4, Name = "Delicious" },
                new Tag { ID = 5, Name = "Fruity" },
                new Tag { ID = 6, Name = "Artery Clogging" },
                new Tag { ID = 7, Name = "Easy" },
                new Tag { ID = 8, Name = "Wanna Play Gwent?" }
            };

            tags.ForEach(t => context.Tags.AddOrUpdate(ta => ta.ID, t));

            context.SaveChanges();

            context.Users.ToList().ForEach(u => context.Recipes.ToList().ForEach(r => r.ApplicationUser = u));

            var recipe1 = context.Recipes.Find(1);
            var recipe2 = context.Recipes.Find(2);
            var recipe3 = context.Recipes.Find(3);
            var recipe4 = context.Recipes.Find(4);
            var recipe5 = context.Recipes.Find(5);

            var ingredient1 = context.Ingredients.Find(1);
            var ingredient2 = context.Ingredients.Find(2);
            var ingredient3 = context.Ingredients.Find(3);
            var ingredient4 = context.Ingredients.Find(4);
            var ingredient5 = context.Ingredients.Find(5);
            var ingredient6 = context.Ingredients.Find(6);
            var ingredient7 = context.Ingredients.Find(7);

            var tag1 = context.Tags.Find(1);
            var tag2 = context.Tags.Find(2);
            var tag3 = context.Tags.Find(3);
            var tag4 = context.Tags.Find(4);
            var tag5 = context.Tags.Find(5);
            var tag6 = context.Tags.Find(6);
            var tag7 = context.Tags.Find(7);
            var tag8 = context.Tags.Find(8);

            recipe1.Ingredients.Add(ingredient1);
            recipe2.Ingredients.Add(ingredient2);
            recipe2.Ingredients.Add(ingredient3);
            recipe2.Ingredients.Add(ingredient4);
            recipe3.Ingredients.Add(ingredient5);
            recipe3.Ingredients.Add(ingredient6);
            recipe4.Ingredients.Add(ingredient3);
            recipe4.Ingredients.Add(ingredient7);
            recipe5.Ingredients.Add(ingredient4);

            recipe1.Tags.Add(tag4);
            recipe1.Tags.Add(tag6);
            recipe2.Tags.Add(tag7);
            recipe3.Tags.Add(tag5);
            recipe3.Tags.Add(tag4);
            recipe4.Tags.Add(tag2);
            recipe4.Tags.Add(tag3);
            recipe5.Tags.Add(tag1);
            recipe5.Tags.Add(tag8);
            recipe4.Tags.Add(tag8);

            recipe1.Status = "approved";
            recipe2.Status = "approved";

            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(recipe1));
            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(recipe2));
            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(recipe3));
            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(recipe4));
            context.Cookbooks.ToList().ForEach(c => c.Recipes.Add(recipe5));

            context.SaveChanges();
        }
    }
}
