using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class MediaEntriesDTO
    {
        public MediaEntriesDTO(MediaEntryDTO[] mediaEntries) 
        {
            this.media = mediaEntries;
        }

        public MediaEntryDTO[] media { get; set; }
    }
}
