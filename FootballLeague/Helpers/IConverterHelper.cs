using FootballLeague.Data.Entities;
using FootballLeague.Models;
using System.Net.Http.Headers;

namespace FootballLeague.Helpers
{
    public interface IConverterHelper
    {
        Club ToClub(ClubViewModel model, string path, bool isNew);

        ClubViewModel ToClubViewModel(Club club);
    }
}
