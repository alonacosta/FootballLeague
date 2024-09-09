using System;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private Random _random; 

        public SeedDb(DataContext dataContext)
        {
            _dataContext = dataContext;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();

            if(! _dataContext.Clubs.Any())
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
                HeadCoach = headCoach
            });
        }
    }
}
