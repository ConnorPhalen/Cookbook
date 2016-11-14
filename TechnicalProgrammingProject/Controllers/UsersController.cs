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
        
        // Could refine these so it is one function that returns an int relating to the role. Would reduce code.
        // Or could keep to use an indivudual functions on other pages for specific validation.
        // | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | | |
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
