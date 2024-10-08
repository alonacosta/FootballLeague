

using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class MatchViewModel : Match
    {
        [Required]
        [Display(Name = "Home Team")]
        public int HomeTeamId { get; set; }

        public IEnumerable<SelectListItem> ClubsHome { get; set; }
        public IEnumerable<SelectListItem> ClubsAway { get; set; }

        public IEnumerable<SelectListItem> Rounds { get; set; }

        [Required]
        [Display(Name = "Away Team")]
        public int AwayTeamId { get; set; }

        public Guid? ImageIdHomeTeam { get; set; }
        public Guid? ImageIdAwayTeam { get; set; }
        public string ImagePathHomeTeam { get; set; }
        public string ImagePathAwayTeam { get; set; }
    }
}
