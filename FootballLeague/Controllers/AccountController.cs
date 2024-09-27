using FootballLeague.Data;
using FootballLeague.Data.Entities;
using FootballLeague.Helpers;
using FootballLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public AccountController(IUserHelper userHelper,
            IFunctionRepository functionRepository,
            IClubRepository clubRepository,
            IStaffMemberRepository staffMemberRepository)
        {
            _userHelper = userHelper;
            _functionRepository = functionRepository;
            _clubRepository = clubRepository;
            _staffMemberRepository = staffMemberRepository;
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
            }

            return RedirectToAction(nameof(GetSuccess));
        }

        public IActionResult GetSuccess()
        {
            return View();
        }
    
    }
}
