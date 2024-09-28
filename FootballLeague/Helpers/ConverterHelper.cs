using FootballLeague.Data.Entities;
using FootballLeague.Models;
using System;

namespace FootballLeague.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Club ToClub(ClubViewModel model, Guid imageId, bool isNew)
        {
            return new Club
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                ImageId = imageId,
                Stadium = model.Stadium,
                Capacity = model.Capacity,
                HeadCoach = model.HeadCoach,                
            };
        }

        public ClubViewModel ToClubViewModel(Club club)
        {
            return new ClubViewModel
            {
                Id = club.Id,
                Name = club.Name,
                ImageId = club.ImageId,
                Stadium = club.Stadium,
                Capacity = club.Capacity,
                HeadCoach = club.HeadCoach,               
            };
        }

        //public UserViewModel ToUserViewModel(User user)
        //{
        //    return new UserViewModel
        //    {
        //        Id = user.Id,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        PhoneNumber = user.PhoneNumber,
        //        ImageId = user.ImageId,                 
        //    };
        //}        

        //public User ToUser(UserViewModel model, Guid imageId, bool isNew)
        //{
        //    return new User
        //    {
        //       Id = isNew ? "" : model.Id,
        //       FirstName = model.FirstName,
        //       LastName = model.LastName,
        //       PhoneNumber = model.PhoneNumber,
        //       ImageId = imageId,
        //    };
        //}
        
    }
}
