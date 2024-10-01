using FootballLeague.Data;
using FootballLeague.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IStaffMemberRepository _staffMemberRepository;

        public UsersController(IUserRepository userRepository,
            IStaffMemberRepository staffMemberRepository) 
        {
            _userRepository = userRepository;
            _staffMemberRepository = staffMemberRepository;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            //Guid imageId = Guid.NewGuid();
          
            var staffMembers = await _staffMemberRepository.GetAllStaffMembers().ToListAsync();

            var userViewModels = staffMembers.Select(staff => new UserViewModel
            {
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
    }
}
