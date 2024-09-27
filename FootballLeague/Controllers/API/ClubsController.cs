using FootballLeague.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FootballLeague.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : Controller
    {
        private readonly IClubRepository _clubRepository;

        public ClubsController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }
        
        [HttpGet]
        public IActionResult GetClubs()
        {
            return Ok(_clubRepository.GetAllClubs());
            //return Ok(_clubRepository.GetAllWithUsers());
        }
    }
}
