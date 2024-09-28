using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class ChangeUserViewModel 
    {

        [Display(Name = "Image")]
        public Guid ImageProfile { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Name")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters")]
        public string PhoneNumber { get; set; }


    }
}
