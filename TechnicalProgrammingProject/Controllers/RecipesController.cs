using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Browse the recipe catalogue.
        /// </summary>
        /// <returns></returns>
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
        
        public ActionResult Search(string searchTerm = "")
        {
            IEnumerable<Recipe> searchResult = db.Recipes.Where(r => searchTerm == ""
                                                    || r.Name.Contains(searchTerm)
                                                    || r.Tags.Any(t => t.Name == searchTerm)
                                                    || r.Ingredients.Any(i => i.Name == searchTerm))
                                                    .OrderBy(r => r.Name);

            if (searchResult == null)
            {
                // There are no results.
                ViewBag.Message = "No Recipes matched those search terms.";
                return View();
            }
            else
            {
                ViewBag.Message = "Returned " + searchResult.Count().ToString() + " results.";
                return View(searchResult);
            }
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

        /// <summary>
        /// View uploads of logged in user or another user.
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns></returns>
        // GET: Recipes/Uploads/{id}
        public ActionResult Uploads(string id)
        {
            //Recipe/Uploads - set to current logged in user
            if (id == null)
            {
                id = User.Identity.GetUserId();
            }
            //get user
            var user = db.Users.Find(id);

            //if user not found
            if (user == null)
            {
                return HttpNotFound();
            }

            //find uploads for the specific user
            var recipes = db.Recipes.Where(r => r.ApplicationUser.Id == id);

            //create model to send to view
            var model = new UploadsViewModel
            {
                UploaderName = user.DisplayName,
                Recipes = recipes.ToList()
            };
            
            //return public upload view
            if (id != User.Identity.GetUserId())
            {
                return View(model);
            }
            //return logged in user's upload view
            return View("CurrentUploads", model);
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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddIngredient()
        {
            var recipe = new Recipe();
            recipe.Ingredients.Add(new Ingredient());

            return PartialView(recipe);
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
