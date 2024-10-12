using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class NextMatchViewModel
    {
        [Display(Name = "Home Team")]
        public string HomeTeamName { get; set; }

        [Display(Name = "Away Team")]
        public string AwayTeamName { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        public Guid ImageIdHomeTeam { get; set; }
        public Guid ImageIdAwayTeam { get; set; }
        public string ImagePathHomeTeam { get; set; }
        public string ImagePathAwayTeam { get; set; }
    }
}
