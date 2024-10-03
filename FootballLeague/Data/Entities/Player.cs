using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Entities
{
    public class Player : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Club")]
        public int ClubId { get; set; }
        public Club Club { get; set; }

        public string ImagePlayerFullPath => ImageId == Guid.Empty
           ? "https://footballleague.blob.core.windows.net/default/no-profile.png"
           : $"https://footballleague.blob.core.windows.net/players/{ImageId}";
    }
}
