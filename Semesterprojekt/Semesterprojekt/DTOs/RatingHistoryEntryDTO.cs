using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class RatingHistoryEntryDTO
    {
        public RatingHistoryEntryDTO(string comment, int star_rating, DateTime created_at, string mediaTitle, bool confirmed)
        {
            this.comment = comment;
            this.star_rating = star_rating;
            this.created_at = created_at;
            this.mediaTitle = mediaTitle;
            this.confirmed = confirmed;
        }

        public string comment { set; get; }
        public int star_rating { set; get; }
        public DateTime created_at { set; get; }
        public string mediaTitle { set; get; }
        public bool confirmed { set; get; }
    }
}
