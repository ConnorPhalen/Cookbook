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
        private RepositoryDbContext db = new RepositoryDbContext();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.Recipes.ToList());
        }
    }
}