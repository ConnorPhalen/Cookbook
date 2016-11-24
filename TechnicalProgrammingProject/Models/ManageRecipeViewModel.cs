using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalProgrammingProject.Models
{
    public class ManageRecipeViewModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public List<Tag> Tags { get; set; }
    }
}