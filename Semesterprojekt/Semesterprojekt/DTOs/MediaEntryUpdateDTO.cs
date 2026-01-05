using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class MediaEntryUpdateDTO
    {
        public MediaEntryUpdateDTO(string title, string description, string mediaType, int releaseYear, int ageRestriction, string[] genres)
        {
            this.title = title;
            this.description = description;
            this.mediaType = mediaType;
            this.releaseYear = releaseYear;
            this.ageRestriction = ageRestriction;
            this.genres = genres;
        }

        public string title { get; set; }
        public string description { get; set; }
        public string mediaType { get; set; }
        public int releaseYear { get; set; }
        public int ageRestriction { get; set; }
        public string[] genres { get; set; }
    }
}
