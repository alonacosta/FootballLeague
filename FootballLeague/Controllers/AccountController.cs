using FootballLeague.Data;
using FootballLeague.Data.Entities;
using FootballLeague.Helpers;
using FootballLeague.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vereyon.Web;

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
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly IFlashMessage _flashMessage;

        public AccountController(IUserHelper userHelper,
            IFunctionRepository functionRepository,
            IClubRepository clubRepository,
            IStaffMemberRepository staffMemberRepository,
            IConverterHelper converterHelper, 
            IBlobHelper blobHelper,
            IConfiguration configuration,
            IMailHelper mailHelper,
            IFlashMessage flashMessage)
        {
            _userHelper = userHelper;
            _functionRepository = functionRepository;
            _clubRepository = clubRepository;
            _staffMemberRepository = staffMemberRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _flashMessage = flashMessage;
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
                    //ViewBag.UserPhoto = await GetUserProfileImage();

                    if(this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    var user = await _userHelper.GetUserByEmailAsync(model.Username);

                    if(await _userHelper.IsUserInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }                   
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

                    string resetToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                    string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));
                    string tokenLink = Url.Action("ResetPassword", "Account", new
                    {
                        userid = user.Id,
                        token = encodedToken
                    }, protocol: HttpContext.Request.Scheme);

                    Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                       $"To allow the user, " +
                       $"please click in this link:</br></br><a href = \"{tokenLink}\">Reset Password and Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        await _userHelper.AddUserToRoleAsync(user, function.NamePosition);

                        var staffMember = new StaffMember
                        {
                            User = user,
                            ClubId = model.ClubId,
                            FunctionId = model.FunctionId,
                        };
                        await _staffMemberRepository.CreateAsync(staffMember);

                        var isInRole = await _userHelper.IsUserInRoleAsync(user, function.NamePosition);
                        if (!isInRole)
                        {
                            await _userHelper.AddUserToRoleAsync(user, function.NamePosition);
                        }

                        _flashMessage.Confirmation("The instractions for user has been sent to email");                        
                    }                    

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Already exists the user with this email");
                    return View(model);
                }                
            }

            return RedirectToAction("Index", "Users");             
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
                      
                        _flashMessage.Confirmation("User updated!");
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

        public IActionResult NotAuthorized()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            
            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(model.UserId);

                if (user == null)
                {
                    return NotFound();
                }

                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
                var result = await _userHelper.ResetPasswordAsync(user, decodedToken, model.NewPassword);

                if (result.Succeeded)
                {
                    var emailToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var confirmEmailResult = await _userHelper.ConfirmEmailAsync(user, emailToken);
                    if (confirmEmailResult.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }                   
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetOldPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }

                return this.View();

            }

            return this.View(model);
        }

        public IActionResult ResetOldPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetOldPassword(ResetOldPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }
    }
}
