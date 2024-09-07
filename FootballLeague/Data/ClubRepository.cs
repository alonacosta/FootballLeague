using FootballLeague.Data.Entities;

namespace FootballLeague.Data
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        public ClubRepository(DataContext context) : base(context)
        {
        }
    }
}
