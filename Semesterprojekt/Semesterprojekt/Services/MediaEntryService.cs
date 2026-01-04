using Semesterprojekt.DTOs;
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
        GenreService genreService;

        public MediaEntryService(MediaEntryRepository mediaEntryRepository, GenreService genreService) 
        {
            this.mediaEntryRepository = mediaEntryRepository;
            this.genreService = genreService;
        }

        public MediaEntry? GetMediaEntry(int id)
        {
            return mediaEntryRepository.GetMediaEntry(id);
        }

        public List<MediaEntry> GetAllMediaEntries()
        {
            return mediaEntryRepository.GetAllMediaEntries();
        }

        public void CreateMediaEntry(MediaEntryDTO mediaEntryDTO, int userId)
        {
            int newId = mediaEntryRepository.CreateMediaEntry(mediaEntryDTO, userId);
            MediaEntry mediaEntry = GetMediaEntry(newId);
            foreach (string genreName in mediaEntryDTO.genres)
            {
                Genre genre = genreService.GetOrCreateGenre(genreName);
                mediaEntryRepository.AssignGenreToMediaEntry(genre.GenreId, newId);
            }
        }
    }
}
