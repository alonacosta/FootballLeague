using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
		private readonly DataContext _context;

		public ClubRepository(DataContext context) : base(context)
        {
			_context = context;
		}

        public IQueryable<Club> GetAllClubs()
        {
            return _context.Clubs.AsNoTracking();
        }

        public IEnumerable<SelectListItem> GetComboClubs()
        {
            var list = _context.Clubs.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a club...)",
                Value = "0"
            });

            return list;
        }         

        public async Task<Club> GetClubDoPlayerAsync(Player player)
        {
            return await _context.Clubs
                .Include(p => p.Players)
                .Where(p => p.Id == player.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Club> GetClubeByNameAsync(string ClubName)
        {
            return await _context.Clubs
                .Include(p => p.Players)
                .Where(c => c.Name == ClubName)
                .FirstOrDefaultAsync();
        }

        
    }
}
