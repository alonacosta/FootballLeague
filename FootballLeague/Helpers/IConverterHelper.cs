using FootballLeague.Data.Entities;
using FootballLeague.Models;
using System;
using System.Net.Http.Headers;

namespace FootballLeague.Helpers
{
    public interface IConverterHelper
    {
        ChangeUserViewModel ToChangeUserViewModel(User user);
        Club ToClub(ClubViewModel model, Guid imageId, bool isNew);

        ClubViewModel ToClubViewModel(Club club);        
        
    }
}
