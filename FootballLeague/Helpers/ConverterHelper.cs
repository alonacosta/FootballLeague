﻿using FootballLeague.Data.Entities;
using FootballLeague.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ChangeUserViewModel ToChangeUserViewModel(User user)
        {
            return new ChangeUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageProfile = user.ImageId,
                ImagePath = user.ImageProfileFullPath,
            };
        } 
        
        public Player ToPlayer(PlayerViewModel model, Guid imageId, int clubId, bool isNew)
        {
            return new Player
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                ImageId = imageId,
                ClubId = clubId,               
                PositionId = model.PositionId,                
            };
        }

        public PlayerViewModel ToPlayerViewModel(Player player) 
        {
            return new PlayerViewModel
            { 
                Id = player.Id,
                Name = player.Name,
                ImageId = player.ImageId,
                ClubId = player.ClubId,
                PositionId = player.PositionId,
            };
        }
        
    }
}
