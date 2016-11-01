using System.Collections.Generic;

namespace TechnicalProgrammingProject.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public string ImageURL { get; set; }
        public string Directions { get; set; }
        public int Rating { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}