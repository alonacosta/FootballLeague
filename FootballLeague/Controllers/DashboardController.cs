using FootballLeague.Data;
using FootballLeague.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        
        //GET: Dashboard
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

            ViewBag.MatchesCount = matchesReadyToClose.Count;
            ViewBag.RoundsCount = roundsReadyToClose.Count;
            ViewBag.Total = roundsReadyToClose.Count + matchesReadyToClose.Count;

            return View(model);
        }


        //GET: Dashboard/GetStatistics
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _matchRepository.CalculateStatisticsAsync();

            var model = new DashboardViewModel
            {
                Statistics = statistics,
            };
            return View(model);
        }

        //GET: Dashboard/GetAllStatistics
        public async Task<IActionResult> GetAllStatistics()
        {
            var rounds = _roundRepository.GetAllRounds();
            var allRoundsStatisticts = new List<RoundStatisticsViewModel>();
           
                var statistics = await _matchRepository.CalculateStatisticsAsync();           
           
            var dashboardAllStat = new DashboardViewModel
            {
               Statistics = statistics,
            };
            return View(dashboardAllStat);
        }

        //GET: Dashboard/GetStatisticsByRound/2
        public async Task<IActionResult> GetStatisticsByRound(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var statistics = await _matchRepository.CalculateStatisticsFromRoundAsync(id.Value);           

            var model = new DashboardViewModel
            {
                Statistics = statistics,
            };
            return View(model);
        }       
    }
}
