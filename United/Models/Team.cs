using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace United.Models
{
    public class Team
    {
        public int LeaguePosition { get; set; }
        public string TeamName { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
    }

    public class TeamsViewModel
    {
        public List<Team> Teams { get; set; }
        public List<TeamResult> TeamDetails { get; set; }
    }

    public class TeamResult
    {
        public DateTime GameDate { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string FTHG { get; set; }
        public string FTAG { get; set; }

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