using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Entities
{
    public class Incident : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Match")]
        public int MatchId { get; set; }

        [Required]
        [Display(Name = "Occurence Name")]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string OccurenceName { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EventTime { get; set; }

        public Match Match { get; set; }

        [Required] 
        [Display(Name = "Player")]
        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
