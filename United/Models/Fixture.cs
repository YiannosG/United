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
            dt.Columns.Add("Name", typeof(String));             // 0
            dt.Columns.Add("GoalsScored", typeof(int));         // 1
            dt.Columns.Add("GoalsConceded", typeof(int));       // 2
            dt.Columns.Add("GoalDifference", typeof(int));      // 3
            dt.Columns.Add("Points", typeof(int));              // 4
            dt.Columns.Add("Position", typeof(int));            // 5

            foreach (var fixture in fixtures)
            {
                // "Is the home team's name anywhere in a row?" - "Is the away team's name anywhere in a row?"
                bool containsHomeTeamName = dt.AsEnumerable().Any(row => fixture.HomeTeam == row.Field<String>("Name"));
                bool containsAwayTeamName = dt.AsEnumerable().Any(row => fixture.AwayTeam == row.Field<String>("Name"));

                // HOME team processing_________________________________________________________________________________
                if (containsHomeTeamName)
                {
                    // Don't insert new row, find the existing one and increment its values
                    var difference = fixture.FTHG - fixture.FTAG;
                    var points = 0;
                    var teamName = fixture.HomeTeam;

                    if (fixture.FTR == "H")
                    {
                        points = 3;
                    }
                    else if (fixture.FTR == "D")
                    {
                        points = 1;
                    }
                    // Find the team's row
                    var row = dt.AsEnumerable().Where(data => data.Field<string>("Name") == teamName);
                    var teamRow = row.FirstOrDefault();
                    if (row.Count() == 1)   // There must be just one row containing the team's name
                    {
                        teamRow["GoalsScored"] = (int)teamRow["GoalsScored"] + fixture.FTHG;
                        teamRow["GoalsConceded"] = (int)teamRow["GoalsConceded"] + fixture.FTAG;
                        teamRow["GoalDifference"] = (int)teamRow["GoalDifference"] + difference;
                        teamRow["Points"] = (int)teamRow["Points"] + points;
                    }
                    else
                    {
                        throw new ArgumentException("error, more than one row contains team's name");
                    }
                }
                else
                {
                    // In effect, this section is only going to run once for every team, inserting its row in the table
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

                // AWAY team processing___________________________________________________________________________________
                if (containsAwayTeamName)
                {
                    // Don't insert new row, find the existing one and increment its values
                    var difference = fixture.FTAG - fixture.FTHG;
                    var points = 0;
                    var teamName = fixture.AwayTeam;

                    if (fixture.FTR == "A")
                    {
                        points = 3;
                    }
                    else if (fixture.FTR == "D")
                    {
                        points = 1;
                    }
                    // Find the team's row
                    var row = dt.AsEnumerable().Where(data => data.Field<string>("Name") == teamName);
                    var teamRow = row.FirstOrDefault();
                    if (row.Count() == 1)   // There must be just one row containing the team's name
                    {
                        teamRow["GoalsScored"] = (int)teamRow["GoalsScored"] + fixture.FTAG;
                        teamRow["GoalsConceded"] = (int)teamRow["GoalsConceded"] + fixture.FTHG;
                        teamRow["GoalDifference"] = (int)teamRow["GoalDifference"] + difference;
                        teamRow["Points"] = (int)teamRow["Points"] + points;
                    }
                    else
                    {
                        throw new ArgumentException("error, more than one row contains team's name");
                    }
                }
                else
                {
                    // In effect, this section is only going to run once for every team, inserting its row in the table
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
            // At this point the DataSet is correct but unordered

            // Turn DataSet to List of Teams
            var teamList = ConvertDataTableToList(dt);

            // Go through the Teams List and order them properly
            var orderedTeamList =
                teamList.OrderByDescending(p => p.Points).ThenBy(gd => gd.GoalDifference).ThenBy(gc => gc.GoalsScored).ToList();

            return orderedTeamList;
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

        private static List<Team> ConvertDataTableToList(DataTable dt)
        {
            var convertedList = (from row in dt.AsEnumerable()
                                 select new Team()
                                 {
                                     LeaguePosition = Convert.ToInt32(row["Position"]),
                                     TeamName = Convert.ToString(row["Name"]),
                                     GoalsScored = Convert.ToInt32(row["GoalsScored"]),
                                     GoalsConceded = Convert.ToInt32(row["GoalsConceded"]),
                                     GoalDifference = Convert.ToInt32(row["GoalDifference"]),
                                     Points = Convert.ToInt32(row["Points"])
                                 }).ToList();

            return convertedList;
        }
    }

    
}