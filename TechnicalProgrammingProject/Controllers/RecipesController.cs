using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
                    //get filename
                    //string pic = Path.GetFileName(recipeViewModel.Image.FileName);
                    //get path
                    //string path = Path.Combine(Server.MapPath("~/images/recipe"), pic);
                    
                    //recipeViewModel.Image.SaveAs(path);

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
        /// TODO: Edit a recipe
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
            return View(recipe);
        }

        /// <summary>
        /// TODO: Edit a recipe
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        // POST: Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecipeID,UserID,Name,Description,CookTime,Servings,ImageURL,Directions")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        /// <summary>
        /// View uploaded recipes to delete.
        /// </summary>
        /// <returns></returns>
        // GET: Recipes/Delete/5
        public ActionResult Delete()
        {
            //name, image, dateuploaded, status
            var userID = User.Identity.GetUserId();
            var recipes = db.Recipes.Where(r => r.ApplicationUser.Id == userID)
                                    .Select(r => new UploadedRecipe
                                    {
                                        ID = r.ID,
                                        Name = r.Name,
                                        //Image = r.ImageURL,
                                        DateUploaded = r.DateUploaded,
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
        
        [HttpPost]
        public ActionResult RateRecipe(int? rating, int? recID)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userID = User.Identity.GetUserId();

                // check to see if user has already rated the recipe
                var recipeChck = db.Recipes.Where(r => r.ID == recID).Where(r => r.Ratings.Any(ra => ra.UserID == userID));
                // rateCheck = rateCheck.Where(r => r.Ratings.All(ra => ra.UserID == userID));

                // The user has previously rated
                if (recipeChck.Count() > 0)
                {
                    // Get the single rating out of the recipe
                    Rating rat = recipeChck.Select(r => r.Ratings).Select(ra => ra).Single().First();

                    rat.rateNumber = (int)rating;
                }
                else // The user has not rated
                {
                    List<Rating> ratingList = new List<Rating>()
                    {
                        new Rating { UserID = userID, rateNumber = (int)rating }
                    };
                    
                    ratingList.ForEach(rsub => db.Ratings.AddOrUpdate(ra => ra.ID, rsub));
                    db.SaveChanges();

                    Recipe curRec = db.Recipes.Find(recID);

                    curRec.Ratings.Add(ratingList[0]);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Details", "Recipes", new { id = recID });
        }

        public string getRatingAverage(int? recID)
        {
            double avg = 0;
            var ratinglist = db.Ratings.Where(ra => ra.Recipes.Any(r => r.ID == recID));

            if(ratinglist.Count() > 0)
            {
                // get the average for all the ratings
                avg = ratinglist.Average(ra => ra.rateNumber);
                return avg.ToString() + "/5!";
            }
            else
            {
                // No results, so no ratings for the recipe
                return "No ratings yet!";
            }
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
