using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private readonly DataContext _context;

        public PlayerRepository(DataContext context) : base(context)
        {
            _context = context;
        }       

        public IQueryable<Player> GetAllPlayersDoClub(int clubId)
        {
            return _context.Players
                .Include(c => c.Club)
                .Include(p => p.Position)
                .Where(c => c.ClubId == clubId);                
        }

		
		public IQueryable<Player> GetAllPlayersDoClubWithPosition(int clubId, int positionId)
        {
            return _context.Players
                .Include(c => c.Club)
                .Include(p => p.Position)
                .Where(c => c.ClubId == clubId && c.PositionId == positionId); 
        }


        public async Task<List<Player>> GetAllPlayersDoClubWithPositionAsync(int clubId, int positionId)
        {
            return await _context.Players
                .Include(c => c.Club)
                .Include(p => p.Position)
                .Where(c => c.ClubId == clubId && c.PositionId == positionId)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public IQueryable<Player> GetAllPlayersFromMatch(string NameClubHome, string NameClubAway)
        {
            return _context.Players
                .Include(c => c.Club)
                .Include(p => p.Position)
                .Where(c => c.Club.Name == NameClubHome || c.Club.Name == NameClubAway)
                .OrderBy(p => p.Name);
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
         {
            return await _context.Players
                .Include(c => c.Club)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(p => p.Id == id);
         }

        public IEnumerable<SelectListItem> GetComboPlayers()
        {
            var list = _context.Players                
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                })
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a player...)",
                Value = "0"
            });

            return list;
        }
        //public IEnumerable<SelectListItem> GetPlayersOfTheMatch(string HomeClubName, string AwayClubName)
        //{
        //    var list = _context.Players
        //        .Include(p => p.Club)
        //        .Where(c => c.Name == HomeClubName || c.Name == AwayClubName)
        //        .Select(c => new SelectListItem
        //    {
        //        Text = c.Name,
        //        Value = c.Id.ToString(),
        //    })
        //        .ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select a player...)",
        //        Value = "0"
        //    });

        //    return list;
        //}
    }
}
