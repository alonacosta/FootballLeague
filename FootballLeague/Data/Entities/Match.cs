using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Entities
{
    public class Match : IEntity
    {
        public int Id { get; set; }

        [Required]
        public int RoundId { get; set; }

        [Required]
        [Display(Name = "Home Team")]
        public string HomeTeam { get; set; }

        [Required]
        [Display(Name = "Away Team")]
        public string AwayTeam { get; set; }

        [Required]
        [Display(Name = "Home Score")]
        public int HomeScore { get; set; }

        [Required]
        [Display(Name = "Away Score")]
        public int AwayScore { get; set; }

        public bool IsClosed { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; } 

        public ICollection<Incident> Incidents { get; set; }
        public Round Round { get; set; }
    }
}
