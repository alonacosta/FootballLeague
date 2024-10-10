using FootballLeague.Data.Entities;
using FootballLeague.Models;
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
        private readonly IClubRepository _clubRepository;

        public MatchRepository(DataContext context,
            IClubRepository clubRepository) : base(context)
        {
            _context = context;
            _clubRepository = clubRepository;
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

        public async Task<List<StatisticsViewModel>> CalculateStatisticsAsync()
        {
            var clubs = await _context.Clubs.ToListAsync();

            var matches = await _context.Matches
                 .Include(m => m.Round)                
                 .ToListAsync();

            var statistics = matches
                .SelectMany(m => new[] 
                {
                    new { Club = m.HomeTeam, Scored = m.HomeScore, Conceded = m.AwayScore, IsHome = true, IsClosed = m.IsClosed },
                    new { Club = m.AwayTeam, Scored = m.AwayScore, Conceded = m.HomeScore, IsHome = false, IsClosed = m.IsClosed},
                })
                .GroupBy(m => m.Club)
                .Select(g =>
                {
                    var club = clubs.FirstOrDefault(c => c.Name == g.Key);

                    return new StatisticsViewModel
                    {
                        ClubName = g.Key,
                        ImageId = club.ImageId,
                        ImageFullPath = club.ImageFullPath,
                        TotalMatches = g.Count(),
                        Wins = g.Count(x => (x.IsHome && x.Scored > x.Conceded) || (!x.IsHome && x.Scored > x.Conceded)),
                        Draws = g.Count(x => x.Scored == x.Conceded),
                        Losses = g.Count(x => (x.IsHome && x.Scored < x.Conceded) || (!x.IsHome && x.Scored < x.Conceded)),
                        GoalsScored = g.Sum(x => x.Scored),
                        GoalsConceded = g.Sum(x => x.Conceded),
                        Points = g.Sum(x => x.Scored > x.Conceded ? 3 : x.Scored == x.Conceded ? 1 : 0),
                        Finished = g.Count(x => x.IsClosed),
                        Scheduled = g.Count(x => !x.IsClosed),
					};
                })
                .OrderByDescending(s => s.Points)
                .ToList();

            for(int i = 0; i < statistics.Count; i++)
            {
                statistics[i].Position = i + 1;
            }
            return statistics;
        }

        public async Task<List<StatisticsViewModel>> CalculateStatisticsFromRoundAsync(int roundId)
        {
            var clubs = await _context.Clubs.ToListAsync();

            var matches = await _context.Matches
                 .Include(m => m.Round)
                 .Where(m => m.RoundId == roundId)
                 .ToListAsync();

            var statistics = matches
                .SelectMany(m => new[]
                {
                    new { Club = m.HomeTeam, Scored = m.HomeScore, Conceded = m.AwayScore, IsHome = true, IsClosed = m.IsClosed },
                    new { Club = m.AwayTeam, Scored = m.AwayScore, Conceded = m.HomeScore, IsHome = false, IsClosed = m.IsClosed},
                })
                .GroupBy(m => m.Club)
                .Select(g =>
                {
                    var club = clubs.FirstOrDefault(c => c.Name == g.Key);

                    return new StatisticsViewModel
                    {
                        ClubName = g.Key,
                        ImageId = club.ImageId,
                        ImageFullPath = club.ImageFullPath,
                        TotalMatches = g.Count(),
                        Wins = g.Count(x => (x.IsHome && x.Scored > x.Conceded) || (!x.IsHome && x.Scored > x.Conceded)),
                        Draws = g.Count(x => x.Scored == x.Conceded),
                        Losses = g.Count(x => (x.IsHome && x.Scored < x.Conceded) || (!x.IsHome && x.Scored < x.Conceded)),
                        GoalsScored = g.Sum(x => x.Scored),
                        GoalsConceded = g.Sum(x => x.Conceded),
                        Points = g.Sum(x => x.Scored > x.Conceded ? 3 : x.Scored == x.Conceded ? 1 : 0),
                        Finished = g.Count(x => x.IsClosed),
                        Scheduled = g.Count(x => !x.IsClosed),
                    };
                })
                .OrderByDescending(s => s.Points)
                .ToList();

            for (int i = 0; i < statistics.Count; i++)
            {
                statistics[i].Position = i + 1;
            }
            return statistics;
        }

    }
}
