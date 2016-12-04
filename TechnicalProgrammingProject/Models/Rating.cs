using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalProgrammingProject.Models
{
    public class Rating
    {
        public Rating()
        {
            Recipes = new HashSet<Recipe>();
        }
        //PK
        public int ID { get; set; }
        public string UserID { get; set; }
        public int rateNumber { get; set; }

        //return recipes
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}