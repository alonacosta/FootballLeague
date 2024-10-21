using FootballLeague.Data;
using FootballLeague.Data.Entities;
using FootballLeague.Helpers;
using FootballLeague.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    [Authorize(Roles = "Representative")]
    public class StaffMembersController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IStaffMemberRepository _staffMemberRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IBlobHelper _blobHelper;

        public StaffMembersController(IUserHelper userHelper,
            IStaffMemberRepository staffMemberRepository,
            IClubRepository clubRepository,
            IFunctionRepository functionRepository,
            IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _staffMemberRepository = staffMemberRepository;
            _clubRepository = clubRepository;
            _functionRepository = functionRepository;
            _blobHelper = blobHelper;
        }
       
        // GET: StaffMembers
        public async Task<IActionResult> Index()
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

            var functions = _functionRepository.GetComboFunctions();
            if(functions == null)
            {
                return NotFound();
            }

            var staffMembers = await _staffMemberRepository.GetStaffMembersByClubAsync(clubId);
            if (staffMembers == null)
            {
                return NotFound();
            }

            var model = new List<RoleOfStaffMemberViewModel>();

            foreach (var staff in staffMembers) 
            {
                var modelStaff = new RoleOfStaffMemberViewModel
                {
                    Id = staff.Id,
                    FirstName = staff.User.FirstName,
                    LastName = staff.User.LastName,
                    ImageId = staff.User.ImageId,
                    ImagePath = staff.User.ImageProfileFullPath,
                    FunctionId = staffMember.FunctionId,
                    FunctionName = staff.Function.NamePosition,
                    Functions = functions,
                };

                model.Add(modelStaff);
            }
            return View(model);
        }

        // GET: StaffMembers/Details/5       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();  
            }

            var staffMember = await _staffMemberRepository.GetStaffMemberByIdAsync(id.Value);
            if (staffMember == null) 
            { 
                return NotFound();
            }

            var model = new RoleOfStaffMemberViewModel
            {
                Id = staffMember.Id,
                FirstName = staffMember.User.FirstName,
                LastName = staffMember.User.LastName,
                ImageId = staffMember.User.ImageId,
                ImagePath = staffMember.User.ImageProfileFullPath,
                FunctionId = staffMember.FunctionId,
                FunctionName= staffMember.Function.NamePosition,
            };

            return View(model);
        }

        //GET StaffMembers/Edit/5       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffMember = await _staffMemberRepository.GetStaffMemberByIdAsync(id.Value);
            if (staffMember == null)
            {
                return NotFound();
            }

            var functions = _functionRepository.GetComboFunctions();
            if (functions == null)
            {
                return NotFound();
            }            

            var model = new RoleOfStaffMemberViewModel
            {
                Id = staffMember.Id,
                FirstName = staffMember.User.FirstName,
                LastName = staffMember.User.LastName,
                ImageId = staffMember.User.ImageId,
                ImagePath = staffMember.User.ImageProfileFullPath,
                FunctionId = staffMember.FunctionId,
                FunctionName = staffMember.Function.NamePosition,
                Functions = functions
            };

            return View(model);
        }

        // POST: StaffMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleOfStaffMemberViewModel model)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    Guid imageId = model.ImageId;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }                   

                    var staffMember = await _staffMemberRepository.GetStaffMemberByIdAsync(model.Id);

                    if (staffMember == null) { return NotFound(); }
                    
                    staffMember.FunctionId = model.FunctionId;
                    await _staffMemberRepository.UpdateAsync(staffMember);
                   
                    var function = await _functionRepository.GetByIdAsync(model.FunctionId);
                    if(function == null) { return NotFound(); } 

                    var user = staffMember.User;
                    if (user != null)
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;                       
                        user.ImageId = imageId;

                        var response = await _userHelper.UpdateUserAsync(user);

                        if (response.Succeeded)
                        {
                            ViewBag.UserMessage = "User updated!";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, response.Errors.ToString());
                        }

                        var namePosition = function.NamePosition;
                        await _userHelper.UpdateUserRoleAsync(user, namePosition);
                    }  
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (!await _staffMemberRepository.ExistAsync(model.Id))
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

        // GET: StaffMember/Delete/5

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var staffMember = await _staffMemberRepository.GetStaffMemberByIdAsync(id.Value);

        //    if (staffMember == null)
        //    {
        //        return NotFound();
        //    }    

        //    var model = new RoleOfStaffMemberViewModel
        //    {
        //        Id = staffMember.Id,
        //        FirstName = staffMember.User.FirstName,
        //        LastName = staffMember.User.LastName,
        //        ImageId = staffMember.User.ImageId,
        //        ImagePath = staffMember.User.ImageProfileFullPath,
        //        FunctionId = staffMember.FunctionId,
        //        FunctionName = staffMember.Function.NamePosition,
        //    };

        //    return View(model);          
        //}

        // POST: StaffMember/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var staffMember = await _staffMemberRepository.GetByIdAsync(id);
                           
        //        await _staffMemberRepository.DeleteAsync(staffMember);
        //        return RedirectToAction(nameof(Index)); 
        //}
    }
}
