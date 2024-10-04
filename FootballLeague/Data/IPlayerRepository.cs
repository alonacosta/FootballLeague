using FootballLeague.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        IQueryable<Player> GetAllPlayersDoClub(int clubId);
        Task<Player> GetPlayerByIdAsync(int id);
    }
}
