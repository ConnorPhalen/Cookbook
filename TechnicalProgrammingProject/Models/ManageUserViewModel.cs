using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalProgrammingProject.Models
{
    public class ManageUserViewModel
    {
        public int UserID { get; set; }
        public string UserDisplayName { get; set; }
        public string UserRole { get; set; }
        // public DateTime DateJoined { get; set; }
        // public int NumOfRecipes { get; set; }

        public string newStatus { get; set; }
    }
}