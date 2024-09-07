using FootballLeague.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Club> Clubs { get; set; }
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
