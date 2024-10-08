using FootballLeague.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IIncidentRepository : IGenericRepository<Incident>
    {
        IQueryable<Incident> GetAllIncidents();
        Task<List<Incident>> GetIncidentsFromMatchAsync(int matchId);
        Task<Incident> GetIncidentByIdAsync(int id);
    }
}
