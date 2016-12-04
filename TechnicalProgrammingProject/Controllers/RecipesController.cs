using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

        /// <summary>
        /// View a recipe's details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReturnUrl = Request.UrlReferrer;
            try
            {
                var recipes = db.Recipes.Where(r => r.ID == id).Include(r => r.Ingredients).Single();
                return View(recipes);
            }
            catch (InvalidOperationException)
            {
                return View("RecipeNotFound");
            }
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

        /// <summary>
        /// Returns the upload a recipe page.
        /// </summary>
        /// <returns></returns>
        // GET: Recipes/Create
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Uploads a recipe.
        /// </summary>
        /// <param name="recipeViewModel"></param>
        /// <returns></returns>
        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRecipeViewModel recipeViewModel)
        {
            //passed validation
            if (ModelState.IsValid)
            {
                //new recipe to insert
                Recipe recipe = new Recipe();
                //if image field is filled
                if (recipeViewModel.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //copy to memorystream
                        recipeViewModel.Image.InputStream.CopyTo(ms);
                        //store bytes inside recipe
                        byte[] image = ms.GetBuffer();
                        recipe.ImageURL = image;
                    }
                }
                //find user of created recipe
                var user = db.Users.Find(User.Identity.GetUserId());
                //build rest of recipe
                recipe.Name = recipeViewModel.Name;
                recipe.Description = recipeViewModel.Description;
                recipe.CookTime = recipeViewModel.CookTime;
                recipe.DateUploaded = DateTime.Now;
                recipe.Servings = recipeViewModel.Servings;
                recipe.Status = "Pending";
                recipe.Ingredients = recipeViewModel.Ingredients;
                recipe.ApplicationUser = user;
                recipe.Directions = recipeViewModel.Directions;
                db.Recipes.Add(recipe);
                //save to db
                db.SaveChanges();
                return View("Success");
            }
            //if validation failed, return view with error messages
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

        /// <summary>
        /// Display Edit recipe view with initial recipe details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            EditRecipeViewModel model = new EditRecipeViewModel();

            if (recipe.ImageURL != null)
            {
                model.Image = recipe.ImageURL;
            }
            model.ID = recipe.ID;
            model.Name = recipe.Name;
            model.Description = recipe.Description;
            model.CookTime = recipe.CookTime;
            model.Directions = recipe.Directions;
            model.Servings = recipe.Servings;
            model.Ingredients = recipe.Ingredients;
            model.Tags = recipe.Tags;

            return View(model);
        }

        /// <summary>
        /// TODO: Edit a recipe
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        // POST: Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRecipeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Recipe recipe = db.Recipes.Find(model.ID);

            if (recipe == null)
            {
                return View(model);
            }

            if (model.RecipePicture != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //copy to memorystream
                    model.RecipePicture.InputStream.CopyTo(ms);
                    //store bytes inside recipe
                    byte[] image = ms.GetBuffer();
                    recipe.ImageURL = image;
                }
            }
            //if (ModelState.IsValid)
            //{
            //    if (model.)
            //    db.Entry(recipe).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(model);
        }

        /// <summary>
        /// View uploaded recipes to delete.
        /// </summary>
        /// <returns></returns>
        // GET: Recipes/Delete/5
        public ActionResult Delete()
        {
            var userID = User.Identity.GetUserId();
            var recipes = db.Recipes.Where(r => r.ApplicationUser.Id == userID)
                                    .Select(r => new UploadedRecipe
                                    {
                                        ID = r.ID,
                                        Name = r.Name,
                                        //Image = r.ImageURL,
                                        DateUploaded = r.DateUploaded,
                                        Rating = r.Rating,
                                        Status = r.Status,
                                        isDelete = false
                                    });
            var model = new DeleteRecipeViewModel
            {
                UploadedRecipes = recipes.ToList()
            };

            return View(model);
        }

        /// <summary>
        /// Deletes uploaded recipes.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: Recipes/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DeleteRecipeViewModel model)
        {
            var ids = model.UploadedRecipes.Where(u => u.isDelete == true).Select(u => u.ID);

            var recipes = db.Recipes.Where(r => ids.Any(u => u == r.ID));
            db.Recipes.RemoveRange(recipes.ToList());
            db.SaveChanges();

            return RedirectToAction("Delete");
        }

        /// <summary>
        /// Delete uploaded recipe.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUpload(int id, string returnUrl)
        {
            var recipe = db.Recipes.Where(r => r.ID == id).Include(r => r.Ingredients).Single();
            db.Recipes.Remove(recipe);
            db.SaveChanges();
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Adds an ingredient to a recipe that is to be created.
        /// </summary>
        /// <returns></returns>
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddIngredient()
        {
            var recipe = new Recipe();
            recipe.Ingredients.Add(new Ingredient());

            return PartialView(recipe);
        }

        /// <summary>
        /// Dispose the db context in addition to other objects that need to be disposed.
        /// </summary>
        /// <param name="disposing"></param>
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
