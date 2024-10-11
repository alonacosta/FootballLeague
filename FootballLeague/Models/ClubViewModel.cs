using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class ClubViewModel : Club
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<Match> UpcomingMatches { get; set; }
        public List<TimeLineItem> TimeLineItems { get; set; }
    }
}
