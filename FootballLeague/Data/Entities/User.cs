using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string LastName { get; set; }
        public Guid ImageId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
       
        public string ImageProfileFullPath => ImageId == Guid.Empty
           ? $"https://footballleague.blob.core.windows.net/default/no-profile.png"
           : $"https://footballleague.blob.core.windows.net/users/{ImageId}";

    }
}
