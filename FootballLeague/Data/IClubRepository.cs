using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IClubRepository : IGenericRepository<Club>
    {       
        public IQueryable GetAllClubs();

        IEnumerable<SelectListItem> GetComboClubs();
        Task<Club> GetClubDoPlayerAsync(Player player);
        Task<Club> GetClubeByNameAsync(string ClubName);

    }
}
