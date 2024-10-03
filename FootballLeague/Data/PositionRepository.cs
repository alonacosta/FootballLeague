using FootballLeague.Data.Entities;

namespace FootballLeague.Data
{
    public class PositionRepository : GenericRepository<Position>, IPositionRepository
    {
        public PositionRepository(DataContext context) : base(context)
        {
        }
    }
}
