using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class RatingHistoryDTO
    {
        public RatingHistoryDTO(RatingHistoryEntryDTO[] ratingHistoryEntryDTOs)
        {
            this.ratingHistoryEntryDTOs = ratingHistoryEntryDTOs;
        }

        public RatingHistoryEntryDTO[] ratingHistoryEntryDTOs { get; set; }
    }
}
