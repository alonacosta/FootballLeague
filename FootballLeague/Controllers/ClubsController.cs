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
        private readonly IBlobHelper _blobHelper;        
        private readonly IConverterHelper _converterHelper;

        public ClubsController(IClubRepository clubRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {       
            _clubRepository = clubRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;           
            _converterHelper = converterHelper;
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
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");                }

                var club = _converterHelper.ToClub(model, imageId, true);

                //TODO: Modify to User with role Representative
                club.User = await _userHelper.GetUserByEmailAsync("alona.costa2@gmail.com");
                await _clubRepository.CreateAsync(club);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
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

            var model = _converterHelper.ToClubViewModel(club); 
            return View(model);
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
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {						
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
                    }

                    var club = _converterHelper.ToClub(model, imageId, false);

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
