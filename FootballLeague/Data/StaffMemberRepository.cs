using FootballLeague.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public class StaffMemberRepository : GenericRepository<StaffMember>, IStaffMemberRepository
    {
        private readonly DataContext _context;

        public StaffMemberRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<StaffMember> GetAllStaffMembers()
        {
            return _context.StaffMembers
                .Include(s => s.Club)        
                .Include(s => s.Function)    
                .Include(s => s.User)                  
                .AsNoTracking();
        }

        public async Task<StaffMember> GetStaffMemberAsync(User user)
        {
            return await _context.StaffMembers
                .Include(s => s.Club)
                .Include(s => s.Function)
                .Include(s => s.User)
                .Where(s => s.User.Id == user.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<StaffMember>> GetStaffMembersByClubAsync(int clubId)
        {
            return await _context.StaffMembers
                .Include(s => s.Club)
                .Include(s => s.Function)
                .Include(s => s.User)
                .Where(s => s.Club.Id == clubId && s.Function.NamePosition != "Representative")
                .ToListAsync();
        }

        public async Task<StaffMember> GetStaffMemberByIdAsync(int id)
        {
            return await _context.StaffMembers
                .Include(s => s.Club)
                .Include(s => s.Function)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }


    }
}
