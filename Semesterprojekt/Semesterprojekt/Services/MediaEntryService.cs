using Semesterprojekt.Entities;
using Semesterprojekt.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Services
{
    public class MediaEntryService
    {
        MediaEntryRepository mediaEntryRepository;

        public MediaEntryService(MediaEntryRepository mediaEntryRepository) 
        {
            this.mediaEntryRepository = mediaEntryRepository;
        }

        public MediaEntry? GetMediaEntry(int id)
        {
            return mediaEntryRepository.GetMediaEntry(id);
        }
    }
}
