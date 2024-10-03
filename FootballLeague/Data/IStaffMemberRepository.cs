using FootballLeague.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public interface IStaffMemberRepository : IGenericRepository<StaffMember>
    {
        IQueryable<StaffMember> GetAllStaffMembers();
        Task<StaffMember> GetStaffMember(User user);
    }
}
