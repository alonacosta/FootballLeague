using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public class RoundRepository : GenericRepository<Round>, IRoundRepository
    {
        private readonly DataContext _context;

        public RoundRepository(DataContext context) : base(context)
        {
           _context = context;
        }

        public IEnumerable<SelectListItem> GetComboRounds()
        {
            var list = _context.Rounds.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a round...)",
                Value = "0"
            });

            return list;
        }
        public async Task<Round> UpdateRoundAsync(Round round)
        {

            var existingRound = await _context.Rounds
                .Include(m => m.Matches)
                .FirstOrDefaultAsync(m => m.Id == round.Id);

            if (existingRound == null)
            {
                return null;
            }

            existingRound.Name = round.Name;
            existingRound.DateStart = round.DateStart;          
            

            _context.Rounds.Update(existingRound);
            await _context.SaveChangesAsync();

            return existingRound;
        }

        public async Task<Round> CloseRoundAsync(Round round)
		{

			var existingRound = await _context.Rounds
				.Include(m => m.Matches)
				.FirstOrDefaultAsync(m => m.Id == round.Id);

			if (existingRound == null)
			{
				return null;
			}

            existingRound.DateStart = round.DateStart;
            existingRound.DateEnd = round.DateEnd;
			existingRound.IsClosed = round.IsClosed;

			_context.Rounds.Update(existingRound);
			await _context.SaveChangesAsync();

			return existingRound;
		}

		public List<Round> GetRoundsReadyToClose()
        {
            var roundsNotClosed = _context.Rounds
                .Include(r => r.Matches)
                .Where(r => !r.IsClosed)
                .ToList();

            var roundsReadyToClose = roundsNotClosed
                .Where(r => r.Matches != null && r.Matches.Any() && r.Matches.All(m => m.IsClosed))
                .ToList();
            return roundsReadyToClose;
        }

        public List<Round> GetRoundsIsClosed()
        {
            var roundsIsClosed = _context.Rounds
               .Where(r => r.IsClosed)
               .ToList();

            return roundsIsClosed;

        }
    }
}
