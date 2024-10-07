using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballLeague.Data;
using FootballLeague.Data.Entities;

namespace FootballLeague.Controllers
{
    public class RoundsController : Controller
    {
        //private readonly DataContext _context;
        private readonly IRoundRepository _roundRepository;

        public RoundsController(DataContext context,
            IRoundRepository roundRepository)
        {
            //_context = context;
            _roundRepository = roundRepository;
        }

        // GET: Rounds
        public IActionResult Index()
        {
            return View(_roundRepository.GetAll().OrderByDescending(r => r.DateStart));
        }

        // GET: Rounds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var round = await _roundRepository.GetByIdAsync(id.Value);
            if (round == null)
            {
                return NotFound();
            }

            return View(round);
        }

        // GET: Rounds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateStart,DateEnd,IsClosed")] Round round)
        {
            if (ModelState.IsValid)
            {
                round.IsClosed = false;
                await _roundRepository.CreateAsync(round);
                
                return RedirectToAction(nameof(Index));
            }
            return View(round);
        }

        // GET: Rounds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var round = await _roundRepository.GetByIdAsync(id.Value);
            if (round == null)
            {
                return NotFound();
            }
            return View(round);
        }

        // POST: Rounds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateStart,DateEnd,IsClosed")] Round round)
        {
            if (id != round.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roundRepository.UpdateAsync(round);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _roundRepository.ExistAsync(round.Id))
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
            return View(round);
        }

        // GET: Rounds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var round = await _roundRepository.GetByIdAsync(id.Value);
            if (round == null)
            {
                return NotFound();
            }

            return View(round);
        }

        // POST: Rounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var round = await _roundRepository.GetByIdAsync(id);
            if (round == null)
            {
                return NotFound();
            }
            await _roundRepository.DeleteAsync(round);
            
            return RedirectToAction(nameof(Index));
        }        
    }
}
