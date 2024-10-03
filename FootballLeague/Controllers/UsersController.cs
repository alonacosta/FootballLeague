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
        

        public UsersController(IStaffMemberRepository staffMemberRepository) 
        {
           
            _staffMemberRepository = staffMemberRepository;
           
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {            
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
