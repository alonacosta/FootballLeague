using FootballLeague.Data.Entities;
using FootballLeague.Models;

namespace FootballLeague.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Club ToClub(ClubViewModel model, string path, bool isNew)
        {
            return new Club
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                ImageLogo = path,
                Stadium = model.Stadium,
                Capacity = model.Capacity,
                HeadCoach = model.HeadCoach,
                User = model.User,
            };
        }

        public ClubViewModel ToClubViewModel(Club club)
        {
            return new ClubViewModel
            {
                Id = club.Id,
                Name = club.Name,
                ImageLogo = club.ImageLogo,
                Stadium = club.Stadium,
                Capacity = club.Capacity,
                HeadCoach = club.HeadCoach,
                User = club.User,
            };
        }
    }
}
