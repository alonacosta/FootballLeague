using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Entities
{
    public class StaffMember : IEntity
    {
        public int Id { get; set; }

        [Required]
        public int ClubId { get; set; }
        public Club Club { get; set; }

        [Required]
        public int FunctionId { get; set; }
        public Function Function { get; set; }

        [Required]
        public User User { get; set; }
    }
}
