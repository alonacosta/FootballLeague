using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballLeague.Data;
using FootballLeague.Data.Entities;
using FootballLeague.Helpers;
using Microsoft.AspNetCore.Authorization;
using FootballLeague.Models;

namespace FootballLeague.Controllers
{
    public class PlayersController : Controller
    {
        private readonly DataContext _context;
        private readonly IPlayerRepository _playerRepository;
        private readonly IStaffMemberRepository _staffMemberRepository;
        private readonly IUserHelper _userHelper;
        private readonly IPositionRepository _positionRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public PlayersController(DataContext context,
            IPlayerRepository playerRepository, 
            IStaffMemberRepository staffMemberRepository,
            IUserHelper userHelper,
            IPositionRepository positionRepository,
            IClubRepository clubRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _playerRepository = playerRepository;
            _staffMemberRepository = staffMemberRepository;
            _userHelper = userHelper;
            _positionRepository = positionRepository;
            _clubRepository = clubRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        //GET: Players
        //[Authorize(Roles = "Representative")]
        [Authorize(Roles = "Representative")]
        public async Task<IActionResult> Index(int? id)
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

            var players = _playerRepository.GetAllPlayersDoClub(clubId).OrderBy(p => p.Name);
            if (players == null)
            {
                return NotFound();
            }
            return View(players);            
        }

        // GET: Players/Details/5        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetPlayerByIdAsync(id.Value);
           
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        //GET
        //[HttpGet("GetTeam/{id}")]       
        public IActionResult GetTeam(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var players = _playerRepository.GetAllPlayersDoClub(id.Value).ToList();

            if (players.Count == 0)
            {
                ViewBag.Message = "Club has no players yet";
                return View();

            }
			return View(players);
		}


			// GET: Players/Create
			[Authorize(Roles = "Representative")]
        public async Task<IActionResult> Create()
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

            var model = new PlayerViewModel
            {
                Positions = _positionRepository.GetComboPositions(),
                ClubId = clubId,
                Club = club,
                ClubName = club.Name,
            };
            
            return View(model);
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Representative")]
        public async Task<IActionResult> Create(PlayerViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if(model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "players");
                }

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
                                
                var player = _converterHelper.ToPlayer(model, imageId, clubId, true);

                await _playerRepository.CreateAsync(player);
                
                return RedirectToAction(nameof(Index));
            }            
            return View(model);
        }

        // GET: Players/Edit/5
        [Authorize(Roles = "Representative")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetPlayerByIdAsync(id.Value);
            if (player == null)
            {
                return NotFound();
            }

            var positions = _positionRepository.GetComboPositions();
            var model = _converterHelper.ToPlayerViewModel(player);
            model.Positions = positions;
            model.ClubName = player.Club.Name;
           
            return View(model);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Representative")]
        public async Task<IActionResult> Edit(PlayerViewModel model)
        {  
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "players");
                    }

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
                    
                    var player = _converterHelper.ToPlayer(model, imageId, clubId, false);

                    await _playerRepository.UpdateAsync(player);                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _playerRepository.ExistAsync(model.Id))
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

        // GET: Players/Delete/5
        [Authorize(Roles = "Representative")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetPlayerByIdAsync(id.Value);                
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Representative")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            try
            {
                await _playerRepository.DeleteAsync(player);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{player.Name} is probably being used!!!";
                    ViewBag.ErrorMessage = $"{player.Name} can't be deleted because there are incidents that use it <br/>" +
                    $"First try deleting all the incidents that player has," +
                    $" and delete it again";
                }
                return View("Error");
            }            
        }

        

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
