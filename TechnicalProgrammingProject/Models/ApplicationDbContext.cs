using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace TechnicalProgrammingProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Cookbook> Cookbooks { get; set; }
        public ApplicationDbContext() : base("name=RepositoryDbContext", throwIfV1Schema: false) {}
       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}