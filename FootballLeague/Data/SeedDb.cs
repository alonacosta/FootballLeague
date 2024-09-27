using FootballLeague.Data.Entities;
using FootballLeague.Helpers;
using Microsoft.AspNetCore.Identity;
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
            await _dataContext.Database.EnsureCreatedAsync();

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
            }

            if (!_dataContext.Clubs.Any())
            {
                AddClub("SL Benfica", "Sport Lisboa e Benfica's Stadium", "Bruno Lage");
                AddClub("Sporting CP", "José Alvalade Stadium", "Rúben Amorim");
                AddClub("FC Porto", "Dragon Stadium", "Vítor Bruno");
                AddClub("Boavista F.C.", "The Bessa Stadium", "Cristiano Bacci");
                await _dataContext.SaveChangesAsync();
            }
        }

        private void AddClub(string name, string stadium, string headCoach)
        {
            _dataContext.Clubs.Add(new Entities.Club
            {
                Name = name,
                Stadium = stadium,
                Capacity = _random.Next(40000, 65500),
                HeadCoach = headCoach,
                //User = user
            });
        }
    }
}
