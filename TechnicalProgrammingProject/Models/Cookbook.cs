using System.Collections.Generic;

namespace TechnicalProgrammingProject.Models
{
    public class Cookbook
    {
        public int CookbookID { get; set; }
        public int UserID { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}