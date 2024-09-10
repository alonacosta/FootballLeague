using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FootballLeague.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
