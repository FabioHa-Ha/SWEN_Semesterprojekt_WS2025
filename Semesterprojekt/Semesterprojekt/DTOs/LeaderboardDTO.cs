using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class LeaderboardDTO
    {
        public LeaderboardDTO(LeaderboardEntryDTO[] leaderboard)
        {
            this.leaderboard = leaderboard;
        }

        public LeaderboardEntryDTO[] leaderboard { get; set; }
    }
}
