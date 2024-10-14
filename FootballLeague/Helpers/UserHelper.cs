using FootballLeague.Data.Entities;
using FootballLeague.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
           await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task UpdateUserRoleAsync(User user, string roleName)
        {
            await CheckRoleAsync(roleName);

            if (!await IsUserInRoleAsync(user, roleName))
            {               
                await _userManager.AddToRoleAsync(user, roleName);
            }
            else
            {
                var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                if (currentRole != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, currentRole);
                }

                await _userManager.AddToRoleAsync(user, roleName);
            }           
           
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists) 
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                });
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

      
        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public string GetUserProfileImage(User user)
        {            
            if (user != null)
            {
                return user.ImageProfileFullPath;
            }
            return "https://footballleague.blob.core.windows.net/default/no-profile.png";
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        //public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        //{
        //    return await _userManager.ResetPasswordAsync(user, token, password);
        //}

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            if (user == null || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Invalid user, token, or password."
                });
            }

            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

    }
}
