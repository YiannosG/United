using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public char FTR { get; set; }
        public int HTHG { get; set; }
        public int HTAG { get; set; }
        public char HTR { get; set; }
        public string Referee { get; set; }
    }
}