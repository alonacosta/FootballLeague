using FootballLeague.Data;
using FootballLeague.Helpers;
using FootballLeague.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
       
        private readonly IStaffMemberRepository _staffMemberRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFunctionRepository _functionRepository;
        private readonly IClubRepository _clubRepository;

        public UsersController(IStaffMemberRepository staffMemberRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IFunctionRepository functionRepository,
            IClubRepository clubRepository) 
        {
           
            _staffMemberRepository = staffMemberRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _functionRepository = functionRepository;
            _clubRepository = clubRepository;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {            
            var staffMembers = await _staffMemberRepository.GetAllStaffMembers().ToListAsync();

            var userViewModels = staffMembers.Select(staff => new UserViewModel
            {
                UserId = staff.User.Id,
                ImageId = staff.User.ImageId,
                ImageFullPath = staff.User.ImageProfileFullPath,
                FullName = staff.User.FullName,                
                PhoneNumber = staff.User.PhoneNumber,
                ClubId = staff.ClubId,
                FunctionId = staff.FunctionId,
                Club = staff.Club,
                Function = staff.Function 
            }).ToList();

            return View(userViewModels);
        }

        //GET Users/Edit/5       
        public async Task<IActionResult> Edit(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return NotFound();
            }

            var staffMember = await _staffMemberRepository.GetStaffMemberByUserIdAsync(UserId);
            if (staffMember == null)
            {
                return NotFound();
            }

            var functions = _functionRepository.GetComboFunctions();
            var clubs = _clubRepository.GetComboClubs();
            

            var model = new EditUserViewModel
            {
                UserId = staffMember.User.Id,
                FirstName = staffMember.User.FirstName,
                LastName = staffMember.User.LastName,
                ImageId = staffMember.User.ImageId,
                ImageFullPath = staffMember.User.ImageProfileFullPath,
                PhoneNumber= staffMember.User.PhoneNumber,
                FunctionId = staffMember.FunctionId,
                Function = staffMember.Function,
                ClubId = staffMember.ClubId,
                Club = staffMember.Club,              
                Functions = functions,
                Clubs = clubs
            };

            return View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
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

                    var staffMember = await _staffMemberRepository.GetStaffMemberByUserIdAsync(model.UserId);

                    if (staffMember == null) { return NotFound(); }

                    staffMember.FunctionId = model.FunctionId;
                    staffMember.ClubId = model.ClubId;
                    await _staffMemberRepository.UpdateAsync(staffMember);

                    var function = await _functionRepository.GetByIdAsync(model.FunctionId);
                    if (function == null) { return NotFound(); }

                    var user = staffMember.User;
                  
                    if (user != null)
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.ImageId = imageId;
                        user.PhoneNumber = model.PhoneNumber;

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
                    if (!await _staffMemberRepository.ExistMemberAsync(model.UserId))
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



    }
}
