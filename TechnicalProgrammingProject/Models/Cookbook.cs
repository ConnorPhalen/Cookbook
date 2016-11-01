using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalProgrammingProject.Models
{
    public class Cookbook
    {
        public int CookbookID { get; set; }
        public int UserID { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}