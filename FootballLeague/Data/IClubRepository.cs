using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeague.Data
{
    public interface IClubRepository : IGenericRepository<Club>
    {
        //public IQueryable GetAllWithUsers();
        public IQueryable GetAllClubs();

        IEnumerable<SelectListItem> GetComboClubs();

    }
}
