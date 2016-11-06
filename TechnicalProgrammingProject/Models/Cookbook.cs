using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalProgrammingProject.Models
{
    public class Cookbook
    {
        [Key, ForeignKey("ApplicationUser")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ApplicationUserID { get; set; }
        //return recipes
        public virtual ICollection<Recipe> Recipes { get; set; }
        //return user
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}