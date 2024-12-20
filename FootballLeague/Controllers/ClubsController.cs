﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballLeague.Data;
using FootballLeague.Data.Entities;
using FootballLeague.Helpers;
using FootballLeague.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Vereyon.Web;

namespace FootballLeague.Controllers
{
    public class ClubsController : Controller
    {    
        private readonly IClubRepository _clubRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;        
        private readonly IConverterHelper _converterHelper;
        private readonly IStaffMemberRepository _staffMemberRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IFlashMessage _flashMessage;

        public ClubsController(IClubRepository clubRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IStaffMemberRepository staffMemberRepository,
            IMatchRepository matchRepository,
            IPositionRepository positionRepository,
            IPlayerRepository playerRepository,
            IFlashMessage flashMessage
            )
        {       
            _clubRepository = clubRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;           
            _converterHelper = converterHelper;
            _staffMemberRepository = staffMemberRepository;
            _matchRepository = matchRepository;
            _positionRepository = positionRepository;
            _playerRepository = playerRepository;
            _flashMessage = flashMessage;
        }

        // GET: Clubs
        public IActionResult Index()
        {
            return View(_clubRepository.GetAll().OrderBy(c => c.Name));
        }

        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetByIdAsync(id.Value);
                
            if (club == null)
            {
                return NotFound();
            }

            var matches = await _matchRepository.GetMatchesByClubNameAsync(club.Name);

            var items = new List<TimeLineItem>();

            foreach(var match in matches )
            {
                var score = (match.HomeTeam == club.Name) ? $"{match.HomeScore} : {match.AwayScore}" : $"{match.AwayScore} : {match.HomeScore}";
                var result = match.IsFinished ? score : "";
                var item = new TimeLineItem
                {                   
                    Content =$"{match.MatchName} \n{match.StartDate.ToString("dd.MM.yyyy HH:mm")} \n{match.State} \n{result}",
					DotCss = match.IsFinished ? "state-success" : "state-progress",
                    CssClass = match.IsFinished ? "completed" : "intermediate",
                };

                items.Add(item);
            }
            var model = new ClubViewModel
            {
                Id = club.Id,
                Name = club.Name,
                Stadium = club.Stadium,
                Capacity = club.Capacity,
                ImageId = club.ImageId, 
                HeadCoach = club.HeadCoach,
                UpcomingMatches = matches,
                TimeLineItems = items
            };

            return View(model);
        }

		// GET: Clubs/ClubDetails/5
		[Authorize(Roles = "Representative")]
		public async Task<IActionResult> ClubDetails()
		{
			var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
			if (user == null)
			{
				return NotFound();
			}

			var staffMember = await _staffMemberRepository.GetStaffMemberAsync(user);
			if (staffMember == null)
			{
				return NotFound();
			}

			var clubId = staffMember.ClubId;

			var club = await _clubRepository.GetByIdAsync(clubId);
			if (club == null)
			{
				return NotFound();
			}

			var matches = await _matchRepository.GetMatchesByClubNameAsync(club.Name);

			var items = new List<TimeLineItem>();

			foreach (var match in matches)
			{
				var item = new TimeLineItem
				{
					Content = $"{match.MatchName} \n{match.StartDate.ToString("dd.MM.yyyy HH:mm")} \n{match.State}",
					DotCss = match.IsFinished ? "state-success" : "state-progress",
					CssClass = match.IsFinished ? "completed" : "intermediate",
				};

				items.Add(item);
			}
			var model = new ClubViewModel
			{
				Id = club.Id,
				Name = club.Name,
				Stadium = club.Stadium,
				Capacity = club.Capacity,
				ImageId = club.ImageId,
				HeadCoach = club.HeadCoach,
				UpcomingMatches = matches,
				TimeLineItems = items
			};

			return View(model);
		}

        //GET: Clubs/GetTeam/5        
        [HttpGet]      
        public async Task<IActionResult> GetTeam(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var club = await _clubRepository.GetByIdAsync(id.Value);
            if(club == null) { return NotFound(); } 

            var teamViewModels = new List<TeamViewModel>();

            var positions = _positionRepository.GetAll();
            var players = new List<Player>();

            foreach (var position in positions)
            {
                players = await _playerRepository
                          .GetAllPlayersDoClubWithPositionAsync(club.Id, position.Id);

                if(!players.Any()) continue;

                foreach (var player in players)
                {
                    var model = new TeamViewModel
                    {
                        ImageId = player.ImageId,
                        ImageFullPath = player.ImagePlayerFullPath,
                        Player = player,
                        Club = club,
                        Position = player.Position,
                        PositionId = player.PositionId,
                        PositionName = player.Position.Name,
                    };

                    teamViewModels.Add(model);
                }
            }

            if (!teamViewModels.Any())
            {
                ViewBag.Message = "Club has no players yet";
            }

            return View(teamViewModels);
        }

        // GET: Clubs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
       
        // POST: Clubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ClubViewModel model)
        {    
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
                    }

                    var club = _converterHelper.ToClub(model, imageId, true);

                    await _clubRepository.CreateAsync(club);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _flashMessage.Danger("This Club already exist!");
                }
                return View(model);
            }
            return View(model);
        }

        // GET: Clubs/Edit/5
        [Authorize(Roles = "Admin, Representative")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetByIdAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToClubViewModel(club); 
            return View(model);
        }           

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Representative")]
        public async Task<IActionResult> Edit(ClubViewModel model)
        {  
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {						
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
                    }

                    var club = _converterHelper.ToClub(model, imageId, false);
                   
                    await _clubRepository.UpdateAsync(club);                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _clubRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("ClubNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }

                if (User.IsInRole("Representative"))
                {
                    return RedirectToAction("ClubDetails", new { id = model.Id });
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Clubs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetByIdAsync(id.Value);
                
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            try
            {               
                await _clubRepository.DeleteAsync(club);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex) 
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{club.Name} is probably being used!!!";
                    ViewBag.ErrorMessage = $"{club.Name} can't be deleted because there are players that use it <br/>" +
                    $"First try deleting all the players that are using it," +
                    $" and delete it again";
                }
                return View("Error");
            }
            
        }
    }
}
