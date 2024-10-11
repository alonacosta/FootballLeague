using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IRoundRepository : IGenericRepository<Round>
    {
        IEnumerable<SelectListItem> GetComboRounds();
        IEnumerable<SelectListItem> GetComboRoundsIsNotClosed();
        Task<Round> UpdateRoundAsync(Round round);
        Task<Round> CloseRoundAsync(Round round);

		List<Round> GetRoundsReadyToClose();
        List<Round> GetRoundsIsClosed();
        List<Round> GetAllRounds();




    }
}
