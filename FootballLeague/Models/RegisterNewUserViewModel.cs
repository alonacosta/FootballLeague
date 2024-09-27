using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters")]
        public string PhoneNumber { get; set; }


        [Required]
        [Display(Name = "Club")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a club.")]
        public int ClubId { get; set; }

        public IEnumerable<SelectListItem> Clubs { get; set; }
       

        [Required]
        [Display(Name = "Function")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a function.")]
        public int FunctionId { get; set; }
       
        public IEnumerable<SelectListItem> Functions { get; set; }        
       

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
