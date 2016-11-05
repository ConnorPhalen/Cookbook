using Microsoft.AspNet.Identity.EntityFramework;

namespace TechnicalProgrammingProject.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
    }
}