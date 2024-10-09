using System.Collections.Generic;

namespace FootballLeague.Models
{
    public class RoundStatisticsViewModel
    {
        public string RoundName { get; set; }

        public List<StatisticsViewModel> Statistics { get; set; }
    }
}
