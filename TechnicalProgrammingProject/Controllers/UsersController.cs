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
using System.Web.Security;
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
            if(isSuperAdmin())
            {
                // Return the admin specific links
                return PartialView("_AdminNavPartial");
            }
            else if(isModerator())
            {
                // Return the moderrator specific links
                return PartialView("_ModeratorNavPartial");
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
        
        [Authorize(Roles = "SuperAdmin,Moderator")]
        public ActionResult DisplayApprovedRecipes()
        {
            // Grab all recipes that need approval, make them into a bunch of ManageRecipeViewModel
            var pendingRecipes =
                        from recipes in db.Recipes
                        where recipes.Status == "approved"
                        select new ManageRecipeViewModel
                        {
                            RecipeName = recipes.Name,
                            RecipeID = recipes.ID,
                            DateUploaded = recipes.DateUploaded,
                            Status = recipes.Status,
                            UploadedUserName = recipes.ApplicationUser.DisplayName,
                            UploadedUserID = recipes.ApplicationUser.Id,
                            Tags = recipes.Tags
                        };
            return View(pendingRecipes);
        }

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

                // if the user is amanging an already approved recipe...
                if(recCheck.Status == "approved")
                {
                    recCheck.Status = status;
                    context.SaveChanges();

                    // Now to return the past URL so they stay on the same page. 
                    return RedirectToAction("DisplayApprovedRecipes");
                }
                else // this is a pending/banned recipe
                {
                    recCheck.Status = status;
                    context.SaveChanges();

                    // Now to return the past URL so they stay on the same page. 
                    return RedirectToAction("ApproveRecipe");

                }
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageUsers()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var userList = db.Users.Select(u => new { u.UserName, u.Id, u.DisplayName }).ToList().Where(ul => ul.Id != User.Identity.GetUserId())
                             .Select(e => new ManageUserViewModel()
                             {
                                 UserDisplayName = e.DisplayName,
                                 UserID = e.Id,
                                 UserName = e.UserName,
                                 UserRoles = userManager.GetRoles(e.Id)
                             });

            return View(userList);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult UpdateUserStatus(string userID, string role)
        {
            using (var userMan = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                // Remove the Users current Role
                string[] userRoles = userMan.GetRoles(userID).ToArray();
                var removeResult =  userMan.RemoveFromRoles(userID, userRoles);

                if(removeResult.Succeeded)
                {
                    // Change user to the selected Role
                    var addResult = userMan.AddToRole(userID, role);

                    if(!addResult.Succeeded)
                    {
                        Console.WriteLine("Failed To Change user Role.");
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
                else
                {
                    Console.WriteLine("Failed To Remove user Role.");
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                ViewBag.message = userMan.FindById(userID).DisplayName + "'s Role successfully changed to" + role;
                
                // Now to return the past URL so they stay on the same page. 
                return RedirectToAction("ManageUsers");
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
