using FootballLeague.Data;
using FootballLeague.Helpers;
using FootballLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;
        private readonly IMatchRepository _matchRepository;

        public HomeController(ILogger<HomeController> logger, IUserHelper userHelper, IMatchRepository matchRepository)
        {
            _logger = logger;
            _userHelper = userHelper;
            _matchRepository = matchRepository;
        }

        //GET: Home
        public async Task<IActionResult> Index()
        {
            var statistics = await _matchRepository.CalculateStatisticsAsync();
            var nextMatches = await _matchRepository.GetNextMatchesAsync();

            var model = new DashboardViewModel
            {
                Statistics = statistics,
                NextMatches = nextMatches
            };
           
            return View(model);
        }

        //GET: Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }        
    }
}
