using FootballLeague.Data;
using FootballLeague.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IMatchRepository _matchRepository;

        public DashboardController(IRoundRepository roundRepository, IMatchRepository matchRepository)
        {
            _roundRepository = roundRepository;
            _matchRepository = matchRepository;
        }
        public IActionResult Index()
        {
            var roundsReadyToClose = _roundRepository.GetRoundsReadyToClose();
            var roundsIsClosed = _roundRepository.GetRoundsIsClosed();
            var matchesReadyToClose = _matchRepository.GetMatchesReadyToClose();

            var model = new DashboardViewModel
            {
                RoundsIsClosed = roundsIsClosed,
                RoundsReadyToClose = roundsReadyToClose,
                MatchesReadyToClose = matchesReadyToClose
            };
            return View(model);
        }
    }
}
