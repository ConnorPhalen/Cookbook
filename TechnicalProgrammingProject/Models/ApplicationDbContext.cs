using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace TechnicalProgrammingProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Cookbook> Cookbooks { get; set; }
        public ApplicationDbContext() : base("name=ApplicationDbContext", throwIfV1Schema: false) {}
       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /* Database first fix... I think.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }
        */
        // Caused errors for creating two User things... god damned auto-generated code
        // public System.Data.Entity.DbSet<TechnicalProgrammingProject.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}