using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class LeaderboardEntryDTO
    {
        public LeaderboardEntryDTO(string username, int numberOfRatings)
        {
            this.username = username;
            this.numberOfRatings = numberOfRatings;
        }

        public string username { get; set; }
        public int numberOfRatings { get; set; }
    }
}
