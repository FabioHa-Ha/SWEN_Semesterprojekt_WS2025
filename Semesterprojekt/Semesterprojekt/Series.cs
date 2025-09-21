using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt
{
    class Series : MediaEntry
    {
        public Series(int mediaEntryId, string title, string description, string mediaType, int releaseYear, 
            string genre, int ageRestriction, int creator) : base(mediaEntryId, title, description, mediaType, 
                releaseYear, genre, ageRestriction, creator)
        {
        }
    }
}
