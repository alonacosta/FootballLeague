using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class RoleOfStaffMemberViewModel
    {
        [Required]    
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Name")]
        public IFormFile ImageFile { get; set; }        

        [Required]
        [Display(Name = "Function")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a function.")]
        public int FunctionId { get; set; }

        public string FunctionName { get; set; }

        public IEnumerable<SelectListItem> Functions { get; set; }
    }
}
