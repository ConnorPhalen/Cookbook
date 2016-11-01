using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TechnicalProgrammingProject.Models
{
    public class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext() : base("name=RepositoryDbContext") {}
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Cookbook> Cookbooks { get; set; }
    }
}