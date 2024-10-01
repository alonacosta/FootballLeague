using FootballLeague.Data.Entities;
using System.Linq;

namespace FootballLeague.Data
{
    public interface IStaffMemberRepository : IGenericRepository<StaffMember>
    {
        IQueryable<StaffMember> GetAllStaffMembers();
    }
}
