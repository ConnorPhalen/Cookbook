using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using TechnicalProgrammingProject.Models;

[assembly: OwinStartup(typeof(TechnicalProgrammingProject.Startup))]

namespace TechnicalProgrammingProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // createRolesandUsers(); // Probably delete later
        }

        /*
        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup create the SuperAdmin Role with a default Admin User    
            if (!roleManager.RoleExists("SuperAdmin"))
            {

                // first we create SuperAdmin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "SuperAdmin";
                roleManager.Create(role);
                
                //Here we create a SuperAdmin user who will maintain the website 
                var user = new ApplicationUser();
                user.UserName = "HulkHogan";
                user.Email = "HulkHogan@WWF.com";

                string userPWD = "HulkHogan@42";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role SuperAdmin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "SuperAdmin");
                }
            }

            // Create Moderator role    
            if (!roleManager.RoleExists("Moderator"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Moderator";
                roleManager.Create(role);
                
                //Here we create a Moderator user who will moderate the website 
                var user = new ApplicationUser();
                user.UserName = "RandyOrton";
                user.Email = "RandyOrton@WWF.com";

                string userPWD = "RandyOrton@42";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Moderator   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Moderator");
                }
            }

            // Create User role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
                
                //Here we create a User who will uplaod some recipes
                var user = new ApplicationUser();
                user.UserName = "Weeb";
                user.Email = "Weeb@anime.com";

                string userPWD = "ilovebodypillows@42";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role User   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "User");
                }
            }

            context.SaveChanges();
        }
        */
    }
}