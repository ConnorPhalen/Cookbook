using System.Collections.Generic;

namespace TechnicalProgrammingProject.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            Recipes = new HashSet<Recipe>();
        }
        //PK
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Index { get; set; }
        //return recipes
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}