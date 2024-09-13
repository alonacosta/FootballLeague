using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class ClubViewModel : Club
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }    
    }
}
