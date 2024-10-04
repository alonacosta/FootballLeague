using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeague.Data
{
    public class PositionRepository : GenericRepository<Position>, IPositionRepository
    {
        private readonly DataContext _context;

        public PositionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboPositions()
        {
            var list = _context.Positions.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a country...)",
                Value = "0",
            });
            return list;
        }
    }
}
