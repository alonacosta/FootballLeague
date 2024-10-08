using FootballLeague.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        public IQueryable<Match> GetMatches();
        Task<List<Match>> GetMatchesWithRound(int roundId);
        Task<Match> GetMatchByIdAsync(int id);
        Task<Match> UpdateMatchAsync(Match match);
    }
}
