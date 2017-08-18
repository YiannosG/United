using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;

namespace United.Models
{
    /// <summary>
    /// Model representing a fixture.
    /// </summary>
    public class Fixture
    {
        /// <summary>
        /// Division (will be always Premier in this case)
        /// </summary>
        public string Div { get; set; }

        /// <summary>
        /// Date of game
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Home Team
        /// </summary>
        public string HomeTeam { get; set; }

        /// <summary>
        /// Away Team
        /// </summary>
        public String AwayTeam { get; set; }

        /// <summary>
        /// Full-time Home Goals
        /// </summary>
        public string FTHG { get; set; }

        /// <summary>
        /// Full-time Away Goals
        /// </summary>
        public string FTAG { get; set; }

        /// <summary>
        /// Full-time Result
        /// </summary>
        public string FTR { get; set; }

        /// <summary>
        /// Half-time Home Goals
        /// </summary>
        public string HTHG { get; set; }

        /// <summary>
        /// Half-time Away Goals
        /// </summary>
        public string HTAG { get; set; }

        /// <summary>
        /// Half-time Result
        /// </summary>
        public string HTR { get; set; }

        /// <summary>
        /// Referee name
        /// </summary>
        public string Referee { get; set; }
    }

    public class FixtureVM
    {
        public string Div { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int FTHG { get; set; }
        public int FTAG { get; set; }
        public string FTR { get; set; }
        public int HTHG { get; set; }
        public int HTAG { get; set; }
        public string HTR { get; set; }
        public string Referee { get; set; }

        public static List<Team> ProcessFixtures(List<FixtureVM> fixtures)
        {
            // First create a data table, on which the bulk of our work will be done
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Name");
            dt.Columns.Add("GoalsScored");
            dt.Columns.Add("GoalsConceded");
            dt.Columns.Add("GoalDifference");
            dt.Columns.Add("Points");
            dt.Columns.Add("Position");

            foreach (var fixture in fixtures)
            {
                bool containsHomeTeamName = dt.AsEnumerable().Any(row => fixture.HomeTeam == row.Field<String>("Name"));
                bool containsAwayTeamName = dt.AsEnumerable().Any(row => fixture.AwayTeam == row.Field<String>("Name"));

                if (containsHomeTeamName)
                {
                    
                }
                else
                {
                    var difference = fixture.FTHG - fixture.FTAG;
                    var points = 0;
                    if (fixture.FTR == "H")
                    {
                        points = 3;
                    }
                    else if (fixture.FTR == "D")
                    {
                        points = 1;
                    }
                    dt.Rows.Add(fixture.HomeTeam, fixture.FTHG, fixture.FTAG, difference,
                      points, 0 );
                }

                if (containsAwayTeamName)
                {
                    
                }
                else
                {
                    var difference = fixture.FTAG - fixture.FTHG;
                    var points = 0;
                    if (fixture.FTR == "A")
                    {
                        points = 3;
                    }
                    else if (fixture.FTR == "D")
                    {
                        points = 1;
                    }
                    dt.Rows.Add(fixture.AwayTeam, fixture.FTAG, fixture.FTHG, difference,
                        points, 0);
                }
            }

            return null;
        }

        public static List<FixtureVM> GetCsvData(HttpPostedFileBase file)
        {
            List<FixtureVM> fixtures = new List<FixtureVM>();

            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;

                    System.IO.File.Delete(path);    // Delete old file from working directory if any, no exception thrown
                    file.SaveAs(path);

                    var csv = new CsvReader(new StreamReader(path));
                    var csvFixtures = csv.GetRecords<Fixture>();

                    foreach (var csvFixture in csvFixtures)
                    {
                        FixtureVM fixtureViewmodel = new FixtureVM();

                        fixtureViewmodel.Div = csvFixture.Div;
                        // Date parsing might fail if format is diffent than what we expect so put it in a try block
                        try
                        {
                            fixtureViewmodel.Date = DateTime.Parse(csvFixture.Date);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        fixtureViewmodel.HomeTeam = csvFixture.HomeTeam;
                        fixtureViewmodel.AwayTeam = csvFixture.AwayTeam;
                        fixtureViewmodel.FTHG = int.Parse(csvFixture.FTHG);
                        fixtureViewmodel.FTAG = int.Parse(csvFixture.FTAG);
                        fixtureViewmodel.FTR = csvFixture.FTR;
                        fixtureViewmodel.HTHG = int.Parse(csvFixture.HTHG);
                        fixtureViewmodel.HTAG = int.Parse(csvFixture.HTAG);
                        fixtureViewmodel.HTR = csvFixture.HTR;
                        fixtureViewmodel.Referee = csvFixture.Referee;

                        fixtures.Add(fixtureViewmodel);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return fixtures;
        } 
    }

    
}