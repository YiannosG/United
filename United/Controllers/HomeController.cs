using System.Collections.Generic;
using System.IO;
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
            var ext = Path.GetExtension(file.FileName);
            if (ext == ".csv")
            {
                // Get List of fixture Viewmodels here, directly from the CSV data
                List<FixtureVM> fixtures = FixtureVM.GetCsvData(file);
                Session.Add("Fixtures", fixtures);

                // Process fixtures here to determine final league table of Teams
                List<Team> teams = FixtureVM.ProcessFixtures(fixtures);
                var teamsViewModel = new TeamsViewModel();

                teamsViewModel.Teams = teams;
                teamsViewModel.AllTeamsDetails = TeamResult.ProcessTeamFixtures(fixtures);

                return View(teamsViewModel);
            }
            else
            {
                ViewBag.Message = "You can only upload .csv files!";
                return View();
            }
        }

        /// <summary>
        /// Controller action for the partial view of Team Details (modal)
        /// </summary>
        /// <param name="teamName">The name of the team</param>
        /// <returns>Details for the given team</returns>
        public ActionResult Details(string teamName)
        {
            List<TeamResult> allDetails = new List<TeamResult>();
            allDetails = TeamResult.ProcessTeamFixtures((List<FixtureVM>)Session["Fixtures"]);

            var teamDetails = FixtureVM.Find(allDetails, teamName);
            ViewBag.TeamName = teamName;
            return PartialView("_Details", teamDetails);
        }   
        
    }
}
