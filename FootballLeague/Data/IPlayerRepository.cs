using FootballLeague.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        IQueryable<Player> GetAllPlayersDoClub(int clubId);
        IQueryable<Player> GetAllPlayersDoClubWithPosition(int clubId, int positionId);
        Task<Player> GetPlayerByIdAsync(int id);
    }
}
