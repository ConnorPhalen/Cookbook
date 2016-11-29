using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalProgrammingProject.Models
{
    public class ManageUserViewModel
    {
        public string UserDisplayName { get; set; }
        public string UserID { get; set; }
        public IList<string> UserRoles { get; set; }
        public string UserName { get; set; }
        public DateTime? DateJoined { get; set; }
        public int NumofRecipes { get; set; }

        public string newRole { get; set; }
    }
}
