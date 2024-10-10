using FootballLeague.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FootballLeague.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : ControllerBase
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IMatchRepository _matchRepository;

        public DashboardController(IMatchRepository matchRepository)
        {           
            _matchRepository = matchRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = await _matchRepository.CalculateStatisticsAsync();

            return Ok(statistics);
        }
    }
}
