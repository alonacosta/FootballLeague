using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FootballLeague.Data
{
    public interface IRoundRepository : IGenericRepository<Round>
    {
        IEnumerable<SelectListItem> GetComboRounds();
    }
}
