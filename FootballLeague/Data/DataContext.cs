using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FootballLeague.Data
{
	public class DataContext : IdentityDbContext<User>
	{		
		public DbSet<Club> Clubs { get; set; }
        public DbSet<Function> Functions { get; set; }
		public DbSet<StaffMember> StaffMembers { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<Position> Positions { get; set; }
        public DbSet<Round> Rounds { get; set; }
		public DbSet<Match> Matches { get; set; }

		public DbSet<Incident> Incidents { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}


        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.NoAction;
            }

            modelBuilder.Entity<Club>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Round>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Function>()
                .HasIndex(c => c.NamePosition)
                .IsUnique();

            modelBuilder.Entity<Position>()
                .HasIndex(c => c.Name)
                .IsUnique();

            //modelBuilder.Entity<Player>()
            //    .HasIndex(c => c.Name)
            //    .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
