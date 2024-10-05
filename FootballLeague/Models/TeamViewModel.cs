using FootballLeague.Data.Entities;
using System;

namespace FootballLeague.Models
{
    public class TeamViewModel
    {   
        public Guid ImageId { get; set; }
        public string ImageFullPath { get; set; }
        public Player Player { get; set; }

        public Club Club { get; set; }
        public Position Position { get; set; }
        public int PositionId { get; set; }

        public string PositionName { get; set; }

    }
}
