using FootballLeague.Data.Entities;
using FootballLeague.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        public IQueryable<Match> GetMatches();
        Task<List<Match>> GetMatchesWithRound(int roundId);
        Task<List<Match>> GetMatchesByClubNameAsync(string name);        
        Task<Match> GetMatchByIdAsync(int id);
        Task<Match> UpdateMatchAsync(Match match);
        Task<Match> CloseMatchAsync(Match match);
        IEnumerable<SelectListItem> GetComboMatches();
        List<Match> GetMatchesReadyToClose();
        Task<List<NextMatchViewModel>> GetNextMatchesAsync();
        Task<List<StatisticsViewModel>> CalculateStatisticsAsync();
        Task<List<StatisticsViewModel>> CalculateStatisticsFromRoundAsync(int roundId);
    }
}
