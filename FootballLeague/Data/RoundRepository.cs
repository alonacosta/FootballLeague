using FootballLeague.Data.Entities;

namespace FootballLeague.Data
{
    public class RoundRepository : GenericRepository<Round>, IRoundRepository
    {
        public RoundRepository(DataContext context) : base(context)
        {
        }
    }
}
