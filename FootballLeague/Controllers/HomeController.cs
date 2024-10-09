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

        public HomeController(ILogger<HomeController> logger, IUserHelper userHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
               
                if (await _userHelper.IsUserInRoleAsync(user, "SportsSecretary"))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                   
                    return View(); 
                }               
            }
            return View();
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
