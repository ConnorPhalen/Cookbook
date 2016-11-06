using System.Collections.Generic;

namespace TechnicalProgrammingProject.Models
{
    public class Ingredient
    {
        //PK
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        //return recipes
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}