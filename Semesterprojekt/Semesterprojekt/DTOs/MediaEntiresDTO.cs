using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class MediaEntiresDTO
    {
        public MediaEntiresDTO(MediaEntryDTO[] mediaEntries) 
        {
            this.media = mediaEntries;
        }

        public MediaEntryDTO[] media { get; set; }
    }
}
