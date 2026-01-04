using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class RatingDTO
    {
        public int stars { get; set; }
        public string comment { get; set; }

        public RatingDTO(int stars, string comment)
        {
            this.stars = stars;
            this.comment = comment;
        }
    }
}
