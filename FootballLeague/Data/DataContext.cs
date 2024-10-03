using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Data
{
	public class DataContext : IdentityDbContext<User>
	{		
		public DbSet<Club> Clubs { get; set; }
        public DbSet<Function> Functions { get; set; }
		public DbSet<StaffMember> StaffMembers { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<Position> Positions { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
