using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballLeague.Data;
using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace FootballLeague.Controllers
{
    [Authorize(Roles = "Representative")]
    public class FunctionsController : Controller
    {     
        private readonly IFunctionRepository _functionRepository;

        public FunctionsController(IFunctionRepository functionRepository)
        {           
           _functionRepository = functionRepository;
        }

        // GET: Functions
        public IActionResult Index()
        {
            return View( _functionRepository.GetAll().OrderBy(f => f.NamePosition));            
        }

        // GET: Functions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _functionRepository.GetByIdAsync(id.Value);
            
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // GET: Functions/Create      
        public IActionResult Create()
        {
            return View();
        }

        // POST: Functions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Representative")]
        public async Task<IActionResult> Create([Bind("Id,NamePosition")] Function function)
        {
            if (ModelState.IsValid)
            { await _functionRepository.CreateAsync(function);
               
                return RedirectToAction(nameof(Index));
            }
            return View(function);
        }

        // GET: Functions/Edit/5       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _functionRepository.GetByIdAsync(id.Value);
           
            if (function == null)
            {
                return NotFound();
            }
            return View(function);
        }

        // POST: Functions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Edit(int id, [Bind("Id,NamePosition")] Function function)
        {
            if (id != function.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _functionRepository.UpdateAsync(function);    
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _functionRepository.ExistAsync(id))
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
            return View(function);
        }

        // GET: Functions/Delete/5        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _functionRepository.GetByIdAsync(id.Value);
            
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // POST: Functions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var function = await _functionRepository.GetByIdAsync(id);
            if (function.NamePosition == "Representative" || function.NamePosition == "SportsSecretary")
            {
                ViewBag.Message = "You can delete this position, don´t have permission!";
                return View();
            }
            else
            {
                 try
                {
                    await _functionRepository.DeleteAsync(function);
           
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                    {
                        ViewBag.ErrorTitle = $"{function.NamePosition} is probably being used!!!";
                        ViewBag.ErrorMessage = $"{function.NamePosition} can't be deleted because there are users that use it <br/>" +
                        $"First try to change this function they have to another," +
                        $" and delete it again";
                    }
                    return View("Error");
                }
            } 
        }        
    }
}
