using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TechnicalProgrammingProject.Models;

namespace TechnicalProgrammingProject.Controllers
{
    public class RecipesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recipes
        public ActionResult Index()
        {
            return View(db.Recipes.ToList());
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            var recipes = db.Recipes.Include(r => r.Ingredients);
            Recipe recipe;

            foreach (Recipe r in recipes)
            {
                if (r.ID == id)
                {
                    recipe = r;
                    return View(recipe);
                }
                else
                {
                    Console.WriteLine("BAD");
                }
            }
            Console.WriteLine("No Recipe with that ID.");
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        // GET: Recipes/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRecipeViewModel recipeViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());

                Recipe recipe = new Recipe();
                recipe.Name = recipeViewModel.Name;
                recipe.Description = recipeViewModel.Description;
                recipe.CookTime = recipeViewModel.CookTime;
                recipe.DateUploaded = DateTime.Now;
                recipe.Servings = recipeViewModel.Servings;
                recipe.Status = "Pending";
                recipe.ImageURL = recipeViewModel.ImageURL;
                recipe.Ingredients = recipeViewModel.Ingredients;
                recipe.ApplicationUser = user;
                recipe.Directions = recipeViewModel.Directions;
                db.Recipes.Add(recipe);

                db.SaveChanges();

                return View("Success");
            }
            return View(recipeViewModel);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecipeID,UserID,Name,Description,CookTime,Servings,ImageURL,Directions,Rating")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            db.Recipes.Remove(recipe);
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
