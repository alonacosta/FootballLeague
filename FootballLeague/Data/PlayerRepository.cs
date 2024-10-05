using FootballLeague.Data.Entities;
using Microsoft.EntityFrameworkCore;
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

        //public IQueryable GetAllPlayersWithClubs()
        //{
        //    return _context.Players
        //        .Include(c => c.Club)
        //        .Include(p => p.Position)
        //        .OrderBy(p => p.Name);          
        //}

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

        public async Task<Player> GetPlayerByIdAsync(int id)
         {
            return await _context.Players
                .Include(c => c.Club)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(p => p.Id == id);
         }
    }
}
