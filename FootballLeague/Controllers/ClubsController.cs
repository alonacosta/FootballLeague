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
using FootballLeague.Models;
using System.IO;

namespace FootballLeague.Controllers
{
    public class ClubsController : Controller
    {    
        private readonly IClubRepository _clubRepository;
        private readonly IUserHelper _userHelper;

        public ClubsController(IClubRepository clubRepository,
            IUserHelper userHelper)
        {       
            _clubRepository = clubRepository;
            _userHelper = userHelper;
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

            return View(club);
        }

        // GET: Clubs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClubViewModel model)
        {           
            var path = string.Empty;

            if(model.ImageFile != null && model.ImageFile.Length > 0)
            {
				var guid = Guid.NewGuid().ToString();
				var file = $"{guid}.jpg";

				path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot\\images\\clubs",
                    file);

                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                path = $"~/images/clubs/{file}";
            }
             
             var club = this.ToClub(model, path);

            if (ModelState.IsValid)
            {
                //TODO: Modify to User with role Representative
                club.User = await _userHelper.GetUserByEmailAsync("alona.costa2@gmail.com");
                await _clubRepository.CreateAsync(club);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private Club ToClub(ClubViewModel model, string path)
        {
            return new Club
            {
                Id = model.Id,
                Name = model.Name,
                ImageLogo = path,
                Stadium = model.Stadium,
                Capacity = model.Capacity,
                HeadCoach = model.HeadCoach,
                User = model.User,
            };
        }

        // GET: Clubs/Edit/5
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

            var model = this.ToClubViewModel(club); 
            return View(model);
        }

        private object ToClubViewModel(Club club)
        {
            return new ClubViewModel
            {
                Id = club.Id,
                Name = club.Name,
                ImageLogo = club.ImageLogo,
                Stadium = club.Stadium,
                Capacity = club.Capacity,
                HeadCoach = club.HeadCoach,
                User = club.User,
            };
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClubViewModel model)
        {  
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageLogo;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
						var guid = Guid.NewGuid().ToString();
						var file = $"{guid}.jpg";

						path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\clubs",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/clubs/{file}";
                    }

                    var club = this.ToClub(model, path);

                    //TODO: Modify to User with role Representative
                    club.User = await _userHelper.GetUserByEmailAsync("alona.costa2@gmail.com");
                    await _clubRepository.UpdateAsync(club);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _clubRepository.ExistAsync(model.Id))
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

        // GET: Clubs/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            await _clubRepository.DeleteAsync(club);
            
            return RedirectToAction(nameof(Index));
        }        
    }
}
