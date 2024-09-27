using FootballLeague.Data.Entities;

namespace FootballLeague.Data
{
    public class StaffMemberRepository : GenericRepository<StaffMember>, IStaffMemberRepository
    {
        private readonly DataContext _context;

        public StaffMemberRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
