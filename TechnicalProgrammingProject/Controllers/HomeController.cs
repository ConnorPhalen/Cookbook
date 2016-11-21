using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnicalProgrammingProject.Models;

namespace TechnicalProgrammingProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Home
        public ActionResult Index(string searchTerm = "") 
        {
            IEnumerable<Recipe> searchResult = db.Recipes.Where(r => searchTerm == ""
                                                                || r.Name.Contains(searchTerm)
                                                                || r.Tags.Any(t => t.Name == searchTerm));

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
    }
}