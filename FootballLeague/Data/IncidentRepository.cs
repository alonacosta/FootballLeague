using FootballLeague.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public class IncidentRepository : GenericRepository<Incident>, IIncidentRepository
    {
        private readonly DataContext _context;

        public IncidentRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Incident> GetAllIncidents()
        {
            return _context.Incidents
                .Include(i => i.Match)
                .Include(i => i.Player);               
        }

        public async Task<List<Incident>> GetIncidentsFromMatchAsync(int matchId)
        {
            return await _context.Incidents
                .Include(i=> i.Match)
                .Include(i => i.Player)                
                .Where(m => m.MatchId == matchId)
                .OrderByDescending(m => m.EventTime)
                .ToListAsync();
        }

        public async Task<Incident> GetIncidentByIdAsync(int id)
        {
            return await _context.Incidents
                .Include(i => i.Match)
                .Include(i => i.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
