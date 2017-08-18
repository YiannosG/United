using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using United.Models;

namespace United.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Upload action takes HttpPostedFileBase which is what is returned by
        /// upload widget, maps CSV data, and finally processes it to form a league table
        /// list of teams
        /// </summary>
        /// <param name="file">The uploaded file</param>
        /// <returns>A view with the list of teams that will form the league table</returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            // Get List of fixture Viewmodels here, directly from the CSV data
            List<FixtureVM> fixtures = FixtureVM.GetCsvData(file);

            TempData["Fixtures"] = fixtures;
            // Process fixtures here to determine final league table of Teams
            List<Team> teams = FixtureVM.ProcessFixtures(fixtures);

            return View(teams);
        }

        public ActionResult Details(string teamName)
        {
            List<FixtureVM> fixtures = (List<FixtureVM>) TempData["Fixtures"];
            var results = FixtureVM.Find(fixtures, teamName);
            List<TeamResult> teams = new List<TeamResult>();
            return PartialView("_Details", teams);
        }   
        
    }
}
