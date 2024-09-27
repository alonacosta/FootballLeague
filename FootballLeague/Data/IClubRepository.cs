using FootballLeague.Data.Entities;
using System.Linq;

namespace FootballLeague.Data
{
    public interface IClubRepository : IGenericRepository<Club>
    {
        //public IQueryable GetAllWithUsers();
        public IQueryable GetAllClubs();

	}
}
