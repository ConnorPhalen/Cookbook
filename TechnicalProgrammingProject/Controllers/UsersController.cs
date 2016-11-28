using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        public ActionResult ApproveRecipe()
        {
            // Grab all recipes that need approval, make them into a bunch of ManageRecipeViewModel
            var pendingRecipes =
                        from recipes in db.Recipes
                        where recipes.Status != "approved"
                        // join u in db.Users on recipes.ApplicationUser.Id equals u.Id
                        select new ManageRecipeViewModel { RecipeName = recipes.Name,
                                                            RecipeID = recipes.ID,
                                                            DateUploaded = recipes.DateUploaded,
                                                            Status = recipes.Status,
                                                            UploadedUserName = recipes.ApplicationUser.DisplayName,
                                                            UploadedUserID = recipes.ApplicationUser.Id,
                                                            Tags = recipes.Tags
                        };
            return View(pendingRecipes);
        }

        /*
        [Authorize(Roles = "SuperAdmin,Moderator")]
        public ActionResult DisplayAllRecipes()
        {
            var allRecipes =
                        from recipes in db.Recipes
                        select recipes;

            return View(allRecipes);
        }
        */

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Moderator")]
        public ActionResult UpdateRecipeStatus(string recipeID, string status)
        {
            int recID = int.Parse(recipeID);

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                // Brings back a recipe with the correct ID
                Recipe recCheck = (from r in context.Recipes
                                   where r.ID == recID
                                   select r).Single();

                if(recCheck == null)
                {
                    // Recipe ID was bad, so the request failed
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                recCheck.Status = status;
                context.SaveChanges();

                // Now to return the past URL so they stay on the same page. 
                return RedirectToAction("ApproveRecipe");
            }
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
