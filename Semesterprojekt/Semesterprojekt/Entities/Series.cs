using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Entities
{
    public class Series : MediaEntry
    {
        public Series(int mediaEntryId, string title, string description, int releaseYear, 
            int ageRestriction, int creator) : base(mediaEntryId, title, description, 
                releaseYear, ageRestriction, creator)
        {
        }
    }
}
