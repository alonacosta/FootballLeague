using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class UserViewModel /*: User*/
    {
        public string UserId { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        public string UserName { get; set; }    

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Club")]
        public int ClubId { get; set; }
        public Club Club { get; set; }

        [Display(Name = "Function")]
        public int FunctionId { get; set; }

        public Function Function { get; set; }


      
    }
}
