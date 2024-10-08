using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace FootballLeague.Models
{
    public class IncidentViewModel : Incident
    {
        //public int RoundId { get; set; }
        //public IEnumerable<SelectListItem> Rounds { get; set; }
        public DateTime MatchStartDate { get; set; }
        public IEnumerable<SelectListItem> Matches { get; set; }
        public IEnumerable<SelectListItem> Players { get; set; }
    }
}
