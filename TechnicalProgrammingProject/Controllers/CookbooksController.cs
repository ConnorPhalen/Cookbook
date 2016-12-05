using Microsoft.AspNet.Identity;
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
    [Authorize]
    public class CookbooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Views the logged in users cookbook or another users cookbook.
        /// </summary>
        /// <returns></returns>
        // GET: Cookbooks
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var recipes = db.Recipes.Where(r => r.Cookbooks.Any(c => c.ApplicationUserID == userId));

            if (recipes.ToList().Count == 0)
            {
                return View("Empty");
            }
            return View(recipes.ToList());
        }

        // GET: Cookbooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cookbook cookbook = db.Cookbooks.Find(id);
            if (cookbook == null)
            {
                return HttpNotFound();
            }
            return View(cookbook);
        }

        // GET: Cookbooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cookbooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CookbookID,UserID")] Cookbook cookbook)
        {
            if (ModelState.IsValid)
            {
                db.Cookbooks.Add(cookbook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cookbook);
        }

        // GET: Cookbooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cookbook cookbook = db.Cookbooks.Find(id);
            if (cookbook == null)
            {
                return HttpNotFound();
            }
            return View(cookbook);
        }

        // POST: Cookbooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CookbookID,UserID")] Cookbook cookbook)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cookbook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cookbook);
        }

        // GET: Cookbooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cookbook cookbook = db.Cookbooks.Find(id);
            if (cookbook == null)
            {
                return HttpNotFound();
            }
            return View(cookbook);
        }

        // POST: Cookbooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cookbook cookbook = db.Cookbooks.Find(id);
            db.Cookbooks.Remove(cookbook);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Search(string searchTerm = "")
        {
            var userID = User.Identity.GetUserId();

            var searchResult = db.Recipes.Where(r => r.Cookbooks.Any(c => c.ApplicationUser.Id == userID))
                                                        .Where((r => searchTerm == ""
                                                        || r.Name.Contains(searchTerm)
                                                        || r.Tags.Any(t => t.Name == searchTerm)
                                                        || r.Ingredients.Any(i => i.Name == searchTerm)));

            if (searchResult == null)
            {
                // There are no results.
                ViewBag.Message = "No Recipes matched those search terms.";
                return View();
            }
            else
            {
                ViewBag.Message = "Returned " + searchResult.Count().ToString() + " results.";
                return View(searchResult.ToList());
            }
        }

        /// <summary>
        /// Checks to see of the user has this recipe in their Cookbook.
        /// If true, disply a 'Remove From Cookbook' button.
        /// If false, display an 'Add to Cookbook' button.
        /// </summary>
        /// <param name="recID"></param>
        /// <returns></returns>
        public HtmlString cookbookCheck(int? recID)
        {
            var userID = User.Identity.GetUserId();

            var isInCookbook = db.Recipes.Where(r => r.ID == recID && r.Cookbooks.Any(c => c.ApplicationUser.Id == userID));

            if (isInCookbook.Count() == 0)
            {
                // return true;
                return new HtmlString("<input class=\"btn btn - primary hvr - grow - shadow\" type=\"submit\" value=\"add\" name=\"option\" />");
            }
            else
            {
                //return false;
                return new HtmlString("<input class=\"btn btn - primary hvr - grow - shadow\" type=\"submit\" value=\"remove\" name=\"option\" />");
            }
        }

        /// <summary>
        /// Adds or Removes the Recipe to the current users Cookbook.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult modifyCookbookRecipe(string option, int recID)
        {
            string userID = User.Identity.GetUserId();

            Recipe result = db.Recipes.Where(r => r.ID == recID).Select(r => r).Single();

            Cookbook userCookbook = db.Cookbooks.Where(c => c.ApplicationUser.ID == userID).Select(c => c).Single();
            
            // if the user wants to add the recipe to their cookbook...
            if (option.Equals("add"))
            {
                userCookbook.Recipes.Add(result);
            }
            else
            {
                userCookbook.Recipes.Remove(result);
            }

            return RedirectToAction("Details", "Recipes", new { id = recID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
