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

        public async Task<IActionResult> GetStatisticsByRound(int? id)
        {
            var statistics = await _matchRepository.CalculateStatisticsFromRoundAsync(id.Value);

            var model = new DashboardViewModel
            {
                Statistics = statistics,
            };
            return View(model);
        }

        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _matchRepository.CalculateStatisticsAsync();

            var model = new DashboardViewModel
            {
                Statistics = statistics,
            };
            return View(model);
        }

        public async Task<IActionResult> GetAllStatistics()
        {
            var rounds = _roundRepository.GetAllRounds();
            var allRoundsStatisticts = new List<RoundStatisticsViewModel>();
           
                var statistics = await _matchRepository.CalculateStatisticsAsync();
            //var roundStatistics = new RoundStatisticsViewModel
            //{
            //    Statistics = statistics,

            //    allRoundsStatisticts.Add(roundStatistics);
            //};
           
            var dashboardAllStat = new DashboardViewModel
            {
               Statistics = statistics,
            };
            return View(dashboardAllStat);
        }


        public async Task<IActionResult> GetAllStatisticsByRound()
        {
            var rounds = _roundRepository.GetAllRounds();   
            var allRoundsStatisticts = new List<RoundStatisticsViewModel>();

            foreach (var round in rounds)
            {
                var statistics = await _matchRepository.CalculateStatisticsFromRoundAsync(round.Id);
                var roundStatistics = new RoundStatisticsViewModel
                {
                    RoundName = round.Name,
                    Statistics = statistics,
                };
                allRoundsStatisticts.Add(roundStatistics);
            }
            var dashboardAllStat = new DashboardViewModel
            {
                RoundStatistics = allRoundsStatisticts,
            };
            return View(dashboardAllStat);
        }
    }
}
