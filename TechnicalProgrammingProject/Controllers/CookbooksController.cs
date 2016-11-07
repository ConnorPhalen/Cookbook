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
    public class CookbooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cookbooks
        public ActionResult Index()
        {
            /* Curretly, this would just get all the users uplaoded recipes, not their favourited ones. 
               We need a new column or two in the database for user favourites, but it shouldn't be hard.
             */

            string userID = User.Identity.GetUserId();

            if(userID == null)
            {
                userID = "1b720021-c6df-4a77-b541-608ee812da83";
            }

            var recipes = db.Recipes.ToList();
            var recipeList = new List<Recipe>();

            foreach (Recipe r in recipes)
            {
                if (r.ApplicationUser.Id.Equals(userID))
                {
                    recipeList.Add(r);
                }
            }

            if(recipeList.Count == 0)
            {
                Console.WriteLine("This User has no recipies");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(recipeList);
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
