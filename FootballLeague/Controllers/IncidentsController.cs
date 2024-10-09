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
using Microsoft.AspNetCore.Authorization;

namespace FootballLeague.Controllers
{
    public class IncidentsController : Controller
    {
        private readonly DataContext _context;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IMatchRepository _matchRepository;

        public IncidentsController(DataContext context,
            IIncidentRepository incidentRepository,
            IPlayerRepository playerRepository,
            IClubRepository clubRepository,
            IMatchRepository matchRepository)
        {
            _context = context;
            _incidentRepository = incidentRepository;
            _playerRepository = playerRepository;
            _clubRepository = clubRepository;
            _matchRepository = matchRepository;
        }

        // GET: Incidents       
        public IActionResult Index()
        {            
            return View(_incidentRepository.GetAllIncidents());
        }

        //GET: Incidents/IncidentsFromMatch   
        public async Task<IActionResult> IncidentsFromMatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var incidents = await _incidentRepository.GetIncidentsFromMatchAsync(id.Value);
           
            return View(incidents);
        }

        // GET: Incidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _incidentRepository.GetIncidentByIdAsync(id.Value);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Incidents/Create
        public async Task<IActionResult> Create(int? id)
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
            var model = new IncidentViewModel
            {   Match = match, 
                //Matches = _matchRepository.GetComboMatches(),
                Players = _playerRepository.GetComboPlayers(),
            };
            
            return View(model);
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var player = await _playerRepository.GetPlayerByIdAsync(model.PlayerId);
                var incident = new Incident
                {                   
                    MatchId = model.Match.Id,
                    OccurenceName = model.OccurenceName,
                    EventTime = model.EventTime,
                    //Match = model.Match,
                    Player = player,
                    PlayerId = model.PlayerId,

                };
                await _incidentRepository.CreateAsync(incident);
               
                return RedirectToAction(nameof(Index));
            }
            //ViewData["MatchId"] = new SelectList(_context.Matches, "Id", "Id", incident.MatchId);
            //ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", incident.PlayerId);
            return View(model);
        }

        // GET: Incidents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _incidentRepository.GetIncidentByIdAsync(id.Value);
            if (incident == null)
            {
                return NotFound();
            }

            var model = new IncidentViewModel
            {
                MatchId = incident.MatchId,
                Match = incident.Match,
                OccurenceName = incident.OccurenceName,
                EventTime = incident.EventTime,
                Player = incident.Player,
                PlayerId = incident.PlayerId,
                //Matches = _matchRepository.GetComboMatches(),
                Players = _playerRepository.GetComboPlayers(),
            };
            //ViewData["MatchId"] = new SelectList(_context.Matches, "Id", "Id", incident.MatchId);
            //ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", incident.PlayerId);
            return View(model);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IncidentViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var player = await _playerRepository.GetPlayerByIdAsync(model.PlayerId);
                    
                    var incident = new Incident
                    {
                        Id = model.Id,
                        MatchId = model.Match.Id,
                        OccurenceName = model.OccurenceName,
                        EventTime = model.EventTime,                       
                        PlayerId = model.PlayerId,
                    };
                   await _incidentRepository.UpdateAsync(incident);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _incidentRepository.ExistAsync(model.Id))
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

        // GET: Incidents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
             var incident = await _incidentRepository.GetIncidentByIdAsync(id.Value);
            //var incident = await _context.Incidents
            //    .Include(i => i.Match)
            //    .Include(i => i.Player)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _incidentRepository.GetIncidentByIdAsync(id);
            await _incidentRepository.DeleteAsync(incident);            
           
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("Incidents/GetPlayersAsync")]
        public async Task<JsonResult> GetPlayersAsync(int matchId)
        {
            //var players = new List<Player>();
            var match = await _matchRepository.GetMatchByIdAsync(matchId);
            var nameClubHome = match.HomeTeam;
            var nameClubAway = match.AwayTeam;  

            var players = _playerRepository.GetAllPlayersFromMatch(nameClubHome, nameClubAway);
            return Json(players);
        }

        [HttpGet]
        public async Task<IActionResult> GetIncidentsDateRange(int matchId)
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

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }
    }
}
