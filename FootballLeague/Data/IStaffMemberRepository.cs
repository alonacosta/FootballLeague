using FootballLeague.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IStaffMemberRepository : IGenericRepository<StaffMember>
    {
        IQueryable<StaffMember> GetAllStaffMembers();
        Task<StaffMember> GetStaffMemberAsync(User user);
        Task<List<StaffMember>> GetStaffMembersByClubAsync(int clubId);
        Task<StaffMember> GetStaffMemberByIdAsync(int id);
    }
}
