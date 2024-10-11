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

       
        [Display(Name = "Home Team")]
        public string HomeTeam { get; set; }        

       
        [Display(Name = "Away Team")]
        public string AwayTeam { get; set; }       

        [Required]
        [Display(Name = "Home Score")]
        public int HomeScore { get; set; }

        [Required]
        [Display(Name = "Away Score")]
        public int AwayScore { get; set; }

        public bool IsClosed { get; set; }

        public bool IsFinished { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; } 

        public ICollection<Incident> Incidents { get; set; }
        public Round Round { get; set; }

        [Display(Name = "State")]
        public string MatchState => IsClosed == false ? "Active" : "Completed";

        [Display(Name = "State")]
        public string State => IsFinished == false ? "Scheduled" : "Finished";

        [Display(Name = "Match")]
        public string MatchName => $"{HomeTeam} - {AwayTeam}";

        [Display(Name = "ResultHome")]
        public string MatchResultHome =>  $"{HomeScore} : {AwayScore}" ;

        [Display(Name = "ResultAway")]
        public string MatchResultAway => $"{AwayScore} : {HomeScore}";
    }
}
