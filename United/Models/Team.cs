using System;
using System.Collections.Generic;

namespace United.Models
{
    /// <summary>
    /// Model class representing the team entity
    /// </summary>
    public class Team
    {
        public int LeaguePosition { get; set; }
        public string Image { get; set; }   // Lacking a db this is just a foldername of local file
        public string TeamName { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
    }

    /// <summary>
    /// The view model to be passed onto the main Index view.
    /// It includes both the list of the teams for the final league table,
    /// and also the list of results for each team, to be passed later to
    /// the single team games view
    /// </summary>
    public class TeamsViewModel
    {
        public List<Team> Teams { get; set; }
        public List<TeamResult> AllTeamsDetails { get; set; }
    }

    /// <summary>
    /// Class with properties and methods around the Team Result entity
    /// </summary>
    public class TeamResult
    {
        public DateTime GameDate { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string FTHG { get; set; }
        public string FTAG { get; set; }

        /// <summary>
        /// Processes fixtures producing results for all the teams, getting only the
        /// necessary data for the games
        /// </summary>
        /// <param name="fixtures">The fixtures as received by the CSV file</param>
        /// <returns>A list of all the game results</returns>
        public static List<TeamResult> ProcessTeamFixtures(List<FixtureVM> fixtures)
        {
            List<TeamResult> teamDetails = new List<TeamResult>();

            foreach (var fixture in fixtures)
            {
                TeamResult teamResult = new TeamResult
                {
                    AwayTeam = fixture.AwayTeam,
                    FTAG = fixture.FTAG.ToString(),
                    FTHG = fixture.FTHG.ToString(),
                    GameDate = fixture.Date,
                    HomeTeam = fixture.HomeTeam
                };
                teamDetails.Add(teamResult);
            }

            return teamDetails;
        }
    }
}