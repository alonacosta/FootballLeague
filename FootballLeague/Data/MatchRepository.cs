using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        private readonly DataContext _context;

        public MatchRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Match> GetMatches()
        {
            return _context.Matches
                .Include(m => m.Round)
                .OrderByDescending(m => m.StartDate);
        }
        public async Task<List<Match>> GetMatchesWithRound(int roundId)
        {
            return await _context.Matches
                .Include(m => m.Round)
                .Where(m => m.RoundId == roundId)
                .OrderByDescending(m => m.StartDate)
                .ToListAsync();
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _context.Matches
                .Include(m => m.Round)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Match> UpdateMatchAsync(Match match)
        {

            var existingMatch = await _context.Matches
                .Include(m => m.Round)
                .FirstOrDefaultAsync(m => m.Id == match.Id);

            if (existingMatch == null)
            {
                return null;
            }

            existingMatch.HomeScore = match.HomeScore;
            existingMatch.AwayScore = match.AwayScore;
            existingMatch.IsFinished = match.IsFinished;

            _context.Matches.Update(existingMatch);
            await _context.SaveChangesAsync();

            return existingMatch;       
        }

        public async Task<Match> CloseMatchAsync(Match match)
        {

            var existingMatch = await _context.Matches
                .Include(m => m.Round)
                .FirstOrDefaultAsync(m => m.Id == match.Id);

            if (existingMatch == null)
            {
                return null;
            }
          
            existingMatch.IsClosed = match.IsClosed;

            _context.Matches.Update(existingMatch);
            await _context.SaveChangesAsync();

            return existingMatch;
        }

        public IEnumerable<SelectListItem> GetComboMatches()
        {
            var list = _context.Matches.Select(c => new SelectListItem
            {
                Text = $"{c.HomeTeam} - {c.AwayTeam}",
                Value = c.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a match...)",
                Value = "0"
            });

            return list;
        }


        public List<Match> GetMatchesReadyToClose()
        {
            var matchesNotClosed = _context.Matches.
                Include(m => m.Round)
                .Where(m => !m.IsClosed)
                .ToList();

            var matchesReadyToClose = matchesNotClosed
                .Where(m => m.IsFinished)
                .ToList();

            return matchesReadyToClose;
            
        }
    }
}
