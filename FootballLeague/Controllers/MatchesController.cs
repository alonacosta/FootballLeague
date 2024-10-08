using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballLeague.Data;
using FootballLeague.Data.Entities;
using FootballLeague.Models;

namespace FootballLeague.Controllers
{
    public class MatchesController : Controller
    {
        private readonly DataContext _context;
        private readonly IMatchRepository _matchRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IRoundRepository _roundRepository;

        public MatchesController(DataContext context,
            IMatchRepository matchRepository,
            IClubRepository clubRepository,
            IRoundRepository roundRepository)
        {
            _context = context;
            _matchRepository = matchRepository;
            _clubRepository = clubRepository;
            _roundRepository = roundRepository;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {  
            var matches = _matchRepository.GetMatches();
           
            var modelList = new List<MatchViewModel>();

            foreach (var match in matches)
            {
                var clubHome = await _clubRepository.GetClubeByNameAsync(match.HomeTeam);
                var clubAway = await _clubRepository.GetClubeByNameAsync(match.AwayTeam);

                var model = new MatchViewModel
                {
                    Id = match.Id,
                    Round = match.Round,
                    RoundId = match.RoundId,
                    HomeTeamId = clubHome.Id,
                    AwayTeamId = clubAway.Id,
                    HomeTeam = clubHome.Name,
                    AwayTeam = clubAway.Name,
                    HomeScore = match.HomeScore,
                    AwayScore = match.AwayScore,
                    IsClosed = match.IsClosed,
                    StartDate = match.StartDate,
                    ImageIdHomeTeam = clubHome.ImageId,
                    ImagePathHomeTeam = clubHome.ImageFullPath,
                    ImageIdAwayTeam = clubAway.ImageId,
                    ImagePathAwayTeam = clubAway.ImageFullPath,
                };

                modelList.Add(model);
            }
            return View(modelList);
        }

        //GET: MatchesByRound     
        public async Task<IActionResult> MatchesByRound(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var matches = await _matchRepository.GetMatchesWithRound(id.Value);

            var modelList = new List<MatchViewModel>();

            foreach (var match in matches)
            {
                var clubHome = await _clubRepository.GetClubeByNameAsync(match.HomeTeam);
                var clubAway = await _clubRepository.GetClubeByNameAsync(match.AwayTeam);

                var model = new MatchViewModel
                {
                    Id = match.Id,
                    Round = match.Round,
                    RoundId = match.RoundId,
                    HomeTeamId = clubHome.Id,
                    AwayTeamId = clubAway.Id,
                    HomeTeam = clubHome.Name,
                    AwayTeam = clubAway.Name,
                    HomeScore = match.HomeScore,
                    AwayScore = match.AwayScore,
                    IsClosed = match.IsClosed,
                    StartDate = match.StartDate,
                    ImageIdHomeTeam = clubHome.ImageId,
                    ImagePathHomeTeam = clubHome.ImageFullPath,
                    ImageIdAwayTeam = clubAway.ImageId,
                    ImagePathAwayTeam = clubAway.ImageFullPath,
                };

                modelList.Add(model);
            }
            return View(modelList);
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetMatchByIdAsync(id.Value);
            
            if (match == null)
            {
                return NotFound();
            }

            var clubHome = await _clubRepository.GetClubeByNameAsync(match.HomeTeam);
            var clubAway = await _clubRepository.GetClubeByNameAsync(match.AwayTeam);
            var model = new MatchViewModel
            {
                Id = match.Id,
                RoundId = match.RoundId,                
                HomeTeam = match.HomeTeam,
                AwayTeam = match.AwayTeam,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore,
                StartDate = match.StartDate,
                IsClosed = match.IsClosed,
                Round = match.Round,
                HomeTeamId = clubHome.Id,
                AwayTeamId = clubAway.Id,
                ImageIdHomeTeam = clubHome.ImageId,
                ImagePathHomeTeam = clubHome.ImageFullPath,
                ImageIdAwayTeam = clubAway.ImageId,
                ImagePathAwayTeam = clubAway.ImageFullPath,
            };

            return View(model);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {           
            var model = new MatchViewModel
            {
                ClubsHome = _clubRepository.GetComboClubs(),
                ClubsAway = _clubRepository.GetComboClubs(),
                Rounds = _roundRepository.GetComboRounds(),
            };

            return View(model);
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchViewModel model)
        {           
            var homeTeam = await _clubRepository.GetByIdAsync(model.HomeTeamId);
            var awayTeam = await _clubRepository.GetByIdAsync(model.AwayTeamId);

             model.Round = await _roundRepository.GetByIdAsync(model.RoundId);
             model.HomeTeam = homeTeam.Name;
             model.AwayTeam = awayTeam.Name;
             model.HomeScore = 0;
             model.AwayScore = 0;
             model.ImageIdHomeTeam = homeTeam.ImageId;
             model.ImageIdAwayTeam = awayTeam.ImageId;
             model.ImagePathHomeTeam = homeTeam.ImageFullPath;
             model.ImagePathAwayTeam = awayTeam.ImageFullPath;

            if (ModelState.IsValid)
            {  
                var round = await _roundRepository.GetByIdAsync(model.RoundId);

                var match = new Match
                {
                    RoundId = model.RoundId,
                    HomeTeam = model.HomeTeam,
                    AwayTeam = model.AwayTeam,
                    HomeScore = model.HomeScore, 
                    AwayScore = model.AwayScore,
                    IsClosed = false,
                    StartDate = model.StartDate,                             
                };
                await _matchRepository.CreateAsync(match);
                return RedirectToAction(nameof(Index));
            }
          
            model.ClubsHome = _clubRepository.GetComboClubs();
            model.ClubsAway = _clubRepository.GetComboClubs();
            model.Rounds = _roundRepository.GetComboRounds();
            return View(model);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetMatchByIdAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            var clubHome = await _clubRepository.GetClubeByNameAsync(match.HomeTeam);
            var clubAway = await _clubRepository.GetClubeByNameAsync(match.AwayTeam);

            var model = new MatchViewModel
            {
                Id = match.Id,
                RoundId = match.RoundId,
                HomeTeam = match.HomeTeam,
                AwayTeam = match.AwayTeam,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore,
                IsClosed = match.IsClosed,
                StartDate = match.StartDate,
                Round = match.Round,
                HomeTeamId = clubHome.Id,
                AwayTeamId = clubAway.Id,                
                ClubsHome = _clubRepository.GetComboClubs(),
                ClubsAway = _clubRepository.GetComboClubs(),
                Rounds = _roundRepository.GetComboRounds(),
            };
            return View(model);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var homeTeam = await _clubRepository.GetByIdAsync(model.HomeTeamId);
                    var awayTeam = await _clubRepository.GetByIdAsync(model.AwayTeamId);
                   
                    model.HomeTeam = homeTeam.Name;
                    model.AwayTeam = awayTeam.Name;
                    model.HomeScore = 0;
                    model.AwayScore = 0;
                    model.IsClosed = false;
                    model.ImageIdHomeTeam = homeTeam.ImageId;
                    model.ImageIdAwayTeam = awayTeam.ImageId;
                    model.ImagePathHomeTeam = homeTeam.ImageFullPath;
                    model.ImagePathAwayTeam = awayTeam.ImageFullPath;

                    var match = new Match
                    {
                        Id = model.Id,
                        Round = model.Round,
                        HomeTeam = model.HomeTeam,
                        AwayTeam = model.AwayTeam,
                        HomeScore = model.HomeScore,
                        AwayScore = model.AwayScore,
                        IsClosed = model.IsClosed,
                        StartDate = model.StartDate,
                        RoundId = model.RoundId,                        
                    };
                    
                   await _matchRepository.UpdateAsync(match);                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _matchRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           
            return View(model);
        }

        // GET: Matches/EditScore/5
        public async Task<IActionResult> UpdateScore(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetMatchByIdAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }
           
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateScore(int id, Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _matchRepository.UpdateMatchAsync(match);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _matchRepository.ExistAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetMatchByIdAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            var clubHome = await _clubRepository.GetClubeByNameAsync(match.HomeTeam);
            var clubAway = await _clubRepository.GetClubeByNameAsync(match.AwayTeam);
            var model = new MatchViewModel
            {
                Id = match.Id,
                RoundId = match.RoundId,
                HomeTeam = match.HomeTeam,
                AwayTeam = match.AwayTeam,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore,
                StartDate = match.StartDate,
                IsClosed = match.IsClosed,
                Round = match.Round,
                HomeTeamId = clubHome.Id,
                AwayTeamId = clubAway.Id,
                ImageIdHomeTeam = clubHome.ImageId,
                ImagePathHomeTeam = clubHome.ImageFullPath,
                ImageIdAwayTeam = clubAway.ImageId,
                ImagePathAwayTeam = clubAway.ImageFullPath,
            };

            return View(model);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            //if(match.IsClosed == true)
            //{
            //    ViewBag.ErrorTitle = $"{match.HomeTeam} {match.AwayTeam} already finished!!!";
            //    ViewBag.ErrorMessage = $"{match.HomeTeam} {match.AwayTeam}  can't be deleted  <br/>";
            //}
            await _matchRepository.DeleteAsync(match);
           
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetRoundStartDate(int roundId)
        {            
            var round = await _roundRepository.GetByIdAsync(roundId);

            if (round == null)
            {
                return NotFound();
            }
           
            return Json(new { startDate = round.DateStart.ToString("yyyy-MM-dd HH:mm") });
        }


        [HttpGet]
        public async Task<IActionResult> GetMatchDateRange(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
            {
                return NotFound();
            }

            var startDate = match.StartDate;
            var endDate = match.StartDate.AddHours(2);

            return Json(new { startDate = startDate.ToString("yyyy-MM-dd HH:mm"), endDate = endDate.ToString("yyyy-MM-dd HH:mm") });
        }
    }
}
