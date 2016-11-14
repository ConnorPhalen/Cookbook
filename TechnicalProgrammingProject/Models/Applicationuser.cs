using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TechnicalProgrammingProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        //return recipes
        public virtual ICollection<Recipe> Recipes { get; set; }
        //return cookbook
        public virtual Cookbook Cookbook { get; set; }

        public ApplicationUser()
        {
            Recipes = new HashSet<Recipe>();
            Birthday = System.DateTime.Now;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Biography { get; set; }
        public System.DateTime? Birthday { get; set; }
        public string ProfileImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

    }
}