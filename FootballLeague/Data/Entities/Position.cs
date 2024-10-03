using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Entities
{
    public class Position : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
    }
}
