using FootballLeague.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FootballLeague.Data
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
		private readonly DataContext _context;

		public ClubRepository(DataContext context) : base(context)
        {
			_context = context;
		}

        public IQueryable GetAllClubs()
        {
            return _context.Clubs;
        }

        // Method that returns Clubs with Users on API
        //public IQueryable GetAllWithUsers()
        //{
        //	return _context.Clubs.Include(c => c.User);
        //}
    }
}
