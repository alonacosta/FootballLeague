using FootballLeague.Data.Entities;
using System.Collections.Generic;

namespace FootballLeague.Models
{
    public class DashboardViewModel
    {
        public List<Round> RoundsReadyToClose { get; set; }
        public List<Round> RoundsIsClosed { get; set; }

        public ICollection<Match> MatchesReadyToClose { get; set; }

        public List<StatisticsViewModel> Statistics { get; set; }

        public List<RoundStatisticsViewModel> RoundStatistics { get; set; }

        
        
    }
}
