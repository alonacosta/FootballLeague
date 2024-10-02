using FootballLeague.Data;
using FootballLeague.Data.Entities;
using FootballLeague.Helpers;
using FootballLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IFunctionRepository _functionRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IStaffMemberRepository _staffMemberRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public AccountController(IUserHelper userHelper,
            IFunctionRepository functionRepository,
            IClubRepository clubRepository,
            IStaffMemberRepository staffMemberRepository,
            IConverterHelper converterHelper, 
            IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _functionRepository = functionRepository;
            _clubRepository = clubRepository;
            _staffMemberRepository = staffMemberRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded) 
                {
                    if(this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Functions = _functionRepository.GetComboFunctions(),
                Clubs = _clubRepository.GetComboClubs(),
            };
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var function = await _functionRepository.GetByIdAsync(model.FunctionId);               
               
                await _userHelper.CheckRoleAsync(function.NamePosition);                
               
                var user = await _userHelper.GetUserByEmailAsync(model.Username); 

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        PhoneNumber = model.PhoneNumber,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, function.NamePosition);

                    var staffMember = new StaffMember
                    {
                        User = user,
                        ClubId = model.ClubId,
                        FunctionId = model.FunctionId,
                    };
                    await _staffMemberRepository.CreateAsync(staffMember);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Already exists the user with this email");
                    return View(model);
                }

                var isInRole = await _userHelper.IsUserInRoleAsync(user, function.NamePosition);
                if (!isInRole) 
                {
                    await _userHelper.AddUserToRoleAsync(user, function.NamePosition);
                }
            }

            return RedirectToAction("Index", "Users"); 
            //RedirectToAction(nameof(GetSuccess));
        }

        public IActionResult GetSuccess()
        {
            return View();
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
           
            if (user != null)
            {  
                model = _converterHelper.ToChangeUserViewModel(user);                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                Guid imageId = model.ImageProfile;

                if(model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                if (user != null) 
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.ImageId = imageId;
                    
                    var response = await _userHelper.UpdateUserAsync(user);

                    if(response.Succeeded)
                    {
                        var updatedUser = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                        model = _converterHelper.ToChangeUserViewModel(updatedUser);                        

                        ModelState.Clear();

                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return this.View(model);
        }
    }
}
