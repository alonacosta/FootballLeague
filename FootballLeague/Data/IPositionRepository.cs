using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FootballLeague.Data
{
    public interface IPositionRepository : IGenericRepository<Position>
    {
        IEnumerable<SelectListItem> GetComboPositions();
    }
}
