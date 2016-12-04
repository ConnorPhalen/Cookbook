using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 1000)]
        public int Quantity { get; set; }
        [Required]
        public string Unit { get; set; }
        public string Index { get; set; }
        //return recipes
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}