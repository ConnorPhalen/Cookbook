using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalProgrammingProject.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Ingredients = new HashSet<Ingredient>();
            Tags        = new HashSet<Tag>();
        }
        //PK
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookTime { get; set; }
        public int Servings { get; set; }
        public string ImageURL { get; set; }
        public string Directions { get; set; }
        public int Rating { get; set; }
        public System.DateTime? DateUploaded { get; set; }
        public string Status { get; set; }
        //return user
        public virtual ApplicationUser ApplicationUser { get; set; }
        //return cookbooks
        public virtual ICollection<Cookbook> Cookbooks { get; set; }
        //return ingredients
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        //return tags
        public virtual ICollection<Tag> Tags { get; set; }
    }
}