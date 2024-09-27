using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeague.Data
{
    public interface IFunctionRepository : IGenericRepository<Function>
    {
       IEnumerable<SelectListItem> GetComboFunctions();
    }
}
