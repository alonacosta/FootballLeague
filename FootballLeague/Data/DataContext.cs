﻿using FootballLeague.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Data
{
	public class DataContext : IdentityDbContext<User>
	{
		public DbSet<Club> Clubs { get; set; }
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
