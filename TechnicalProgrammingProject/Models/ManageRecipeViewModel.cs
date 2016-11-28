using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalProgrammingProject.Models
{
    public class ManageRecipeViewModel
    {
        public string RecipeName { get; set; }
        public int RecipeID { get; set; }
        public string UploadedUserName { get; set; }
        public string UploadedUserID { get; set; }
        public DateTime? DateUploaded { get; set; }
        public string Status { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public string newStatus {get; set; }
    }
}