using FootballLeague.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FootballLeague.Models
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath { get; set; }
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Display(Name = "Name")]
        //public string FullName { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        //public string UserName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Club")]
        public int ClubId { get; set; }
        public Club Club { get; set; }

        [Display(Name = "Function")]
        public int FunctionId { get; set; }

        public Function Function { get; set; }
        public IEnumerable<SelectListItem> Functions { get; set; }
        public IEnumerable<SelectListItem> Clubs { get; set; }
    }
}
