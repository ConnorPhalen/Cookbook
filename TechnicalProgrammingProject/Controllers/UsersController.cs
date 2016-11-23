using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TechnicalProgrammingProject.Models;

namespace TechnicalProgrammingProject.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                if (isSuperAdmin())
                {
                    ViewBag.displayMenu = "SuperAdmin";
                }

                if (isModerator())
                {
                    ViewBag.displayMenu = "Moderator";
                }
                if (isUser())
                {
                    ViewBag.displayMenu = "User";
                }

                return View();
            }
            ViewBag.displayMenu = "Not Logged In";
            
            return View();
        }
        
        public ActionResult GetRoleLinks()
        {
            if(isSuperAdmin() || isModerator())
            {
                // Return the admin specific links
                return PartialView("_AdminNavPartial");
            }
            else
            {
                // return any user specific links... though there may not be any
                return PartialView("_UserNavPartial");
            }
        }

        [Authorize(Roles = "SuperAdmin,Moderator")]
        public ActionResult Manage()
        {
            var pendingRecipes =
                        from recipes in db.Recipes
                        where recipes.Status != "approved"
                        select recipes;

            return View(pendingRecipes);

            /* Can get rid of this snce the Authorize attribute does the check for us.
            if (isSuperAdmin() || isModerator())
            {
                var pendingRecipes = 
                        from recipes in db.Recipes
                        where recipes.Status != "approved"
                        select recipes;

                return View(pendingRecipes);
            }

            return RedirectToAction("Index", "Home");
            */
        }
        
        public bool isUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Grabs the role of the logged in user
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());

                if (s[0].ToString() == "User")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isModerator()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Grabs the role of the logged in user
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());

                if (s[0].ToString() == "Moderator")
                {
                    return true;
                }
            }
            return false;
        }

        public bool isSuperAdmin()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Grabs the role of the logged in user
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());

                if (s[0].ToString() == "SuperAdmin")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
