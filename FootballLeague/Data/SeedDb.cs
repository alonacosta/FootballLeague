using FootballLeague.Data.Entities;
using FootballLeague.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
       
        private Random _random; 

        public SeedDb(DataContext dataContext, IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;           
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Representative");
            await _userHelper.CheckRoleAsync("SportsSecretary");

            var user = await _userHelper.GetUserByEmailAsync("alona.costa2@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Alona",
                    LastName = "Costa",
                    Email = "alona.costa2@gmail.com",
                    UserName = "alona.costa2@gmail.com",
                    PhoneNumber = "965965852"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success) 
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var confirmationResult = await _userHelper.ConfirmEmailAsync(user, token);


                if (!confirmationResult.Succeeded)
                {
                    throw new InvalidOperationException("Could not confirm the user's email in seeder");
                }                

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!_dataContext.Clubs.Any())
            {
                AddClub("SL Benfica", "Sport Lisboa e Benfica's Stadium", 65592, "Bruno Lage");
                AddClub("Sporting CP", "José Alvalade Stadium", 50095, "Rúben Amorim");
                AddClub("FC Porto", "Dragon Stadium", 50033, "Vítor Bruno");
                AddClub("Boavista F.C.", "The Bessa Stadium", 28263, "Cristiano Bacci");
                AddClub("Santa Clara", "The Stadium of São Miguel", 12500, "Vasco Matos");
                AddClub("Vítoria SC", "The Stadium Dom Afonso Henriques", 30029, "Rui Borges");
                AddClub("Braga", "The Municipal Stadium of Braga", 30286, "Carlos Carvalhal");
                AddClub("Moreirense", "The Stadium Parque Moreira de Cónegos ", 6150, "Paulo Alves");
                await _dataContext.SaveChangesAsync();
            }

            if (!_dataContext.Functions.Any())
            {
               await _dataContext.Functions.AddRangeAsync(
                    new Function { NamePosition = "Representative" },
                    new Function { NamePosition = "SportsSecretary" },
                    new Function { NamePosition = "Staff" }
                    );
                await _dataContext.SaveChangesAsync();
            }

            if(!_dataContext.Positions.Any())
            {
                await _dataContext.Positions.AddRangeAsync(
                    new Position { Name = "Goalkeeper" },
                    new Position { Name = "Defence" },
                    new Position { Name = "Attack" },
                    new Position { Name = "Midfield" }
                    );
                await _dataContext.SaveChangesAsync();
            }
        }

        private void AddClub(string name, string stadium, int capacity, string headCoach)
        {
            _dataContext.Clubs.Add(new Entities.Club
            {
                Name = name,
                Stadium = stadium,
                Capacity = capacity,
                HeadCoach = headCoach,                
            });
        }
    }
}
