using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class PlayerViewModel : Player
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> Positions { get; set; }

        [Display(Name = "Club")]
        public string ClubName { get; set; }
    }
}
