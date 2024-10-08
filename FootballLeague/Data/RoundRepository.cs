using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
    }
}
