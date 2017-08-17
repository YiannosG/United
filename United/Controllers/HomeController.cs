using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
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

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string path = null;

            List<FixtureVM> fixtures = new List<FixtureVM>();
            List<Team> Teams = new List<Team>();

            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
                    
                    System.IO.File.Delete(path);    // Delete old file from working directory if any, no exception thrown
                    file.SaveAs(path);

                    var csv = new CsvReader(new StreamReader(path));
                    var csvFixtures = csv.GetRecords<Fixture>();

                    foreach (var fixture in csvFixtures)
                    {
                        FixtureVM fixtureViewmodel = new FixtureVM();

                        fixtureViewmodel.Div = fixture.Div;
                        // Date parsing might fail if format is diffent than what we expect so put it in a try block
                        try
                        {
                            fixtureViewmodel.Date = DateTime.Parse(fixture.Date);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            ViewData["error"] = "Date parse failed";
                        }
                        
                        fixtureViewmodel.HomeTeam = fixture.HomeTeam;
                        fixtureViewmodel.AwayTeam = fixture.AwayTeam;
                        fixtureViewmodel.FTHG = int.Parse(fixture.FTHG);
                        fixtureViewmodel.FTAG = int.Parse(fixture.FTAG);
                        fixtureViewmodel.FTR = Convert.ToChar(fixture.FTR);
                        fixtureViewmodel.HTHG = int.Parse(fixture.HTHG);
                        fixtureViewmodel.HTAG = int.Parse(fixture.HTAG);
                        fixtureViewmodel.HTR = Convert.ToChar(fixture.HTR);
                        fixtureViewmodel.Referee = fixture.Referee;

                        fixtures.Add(fixtureViewmodel);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ViewData["error"] = "Upload failed";
            }
            return View();
        }

        /// <summary>
        /// Simplifies the more cumbersome .NET parsing
        /// MyEnum MyVar = (MyEnum) Enum.Parse(typeof(MyEnum), "SomeString", true);
        /// </summary>
        /// <typeparam name="T">Generic class to adapt to enum class</typeparam>
        /// <param name="value">The string to be parsed</param>
        /// <returns>The correct enum selection</returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
