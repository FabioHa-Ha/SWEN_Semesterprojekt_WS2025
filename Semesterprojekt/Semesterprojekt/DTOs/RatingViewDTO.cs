using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class RatingViewDTO
    {
        public RatingViewDTO(string comment, int star_rating, string username, DateTime created_at)
        {
            this.comment = comment;
            this.star_rating = star_rating;
            this.username = username;
            this.created_at = created_at;
        }

        public string comment { set; get; }
        public int star_rating { set; get; }
        public string username { set; get; }
        public DateTime created_at { set; get; }
    }
}
