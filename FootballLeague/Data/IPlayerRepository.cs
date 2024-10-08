using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        IQueryable<Player> GetAllPlayersDoClub(int clubId);
        IQueryable<Player> GetAllPlayersDoClubWithPosition(int clubId, int positionId);
        IQueryable<Player> GetAllPlayersFromMatch(string NameClubHome, string NameClubAway);
        Task<Player> GetPlayerByIdAsync(int id);
        IEnumerable<SelectListItem> GetComboPlayers();
    }
}
