using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeague.Data
{
    public class FunctionRepository : GenericRepository<Function>, IFunctionRepository
    {
        private readonly DataContext _context;

        public FunctionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboFunctions()
        {
            var list = _context.Functions.Select(f => new SelectListItem
            {
                Text = f.NamePosition,
                Value = f.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a function...)",
                Value = "0"
            });

            return list;
        }
    }
}
