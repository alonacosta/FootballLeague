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

        public async Task<IActionResult> Index()
        {
            var statistics = await _matchRepository.CalculateStatisticsAsync();

            var model = new DashboardViewModel
            {
                Statistics = statistics,
            };
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
               
                if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                   
                    return View(model); 
                }         

            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        //[Route("error/404")]
        //public IActionResult Error404()
        //{
        //    return View();
        //}
    }
}
