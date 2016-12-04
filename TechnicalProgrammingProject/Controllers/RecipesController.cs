using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TechnicalProgrammingProject.Models;

namespace TechnicalProgrammingProject.Controllers
{
    [Authorize]
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
       
        // GET: Recipes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                                        Image = r.ImageURL,
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

        [HttpPost]
        public ActionResult DeleteUpload(int id, string returnUrl)
        {
            var recipe = db.Recipes.Where(r => r.ID == id).Include(r => r.Ingredients).Single();
            db.Recipes.Remove(recipe);
            db.SaveChanges();
            return Redirect(returnUrl);
        }
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
