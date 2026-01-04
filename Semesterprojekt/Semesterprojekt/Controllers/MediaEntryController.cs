using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Semesterprojekt.Controllers
{
    public class MediaEntryController
    {
        MediaEntryService mediaEntryService;
        GenreService genreService;

        public MediaEntryController(MediaEntryService mediaEntryService, GenreService genreService)
        {
            this.mediaEntryService = mediaEntryService;
            this.genreService = genreService;
        }

        public string GetMediaEntry(string meidaEntryIdString)
        {
            int mediaEntryId = Int32.Parse(meidaEntryIdString);
            MediaEntry? mediaEntry = mediaEntryService.GetMediaEntry(mediaEntryId);
            if (mediaEntry == null)
            {
                throw new UnkownMediaEntryException("Invalid Id!");
            }
            string[] genres = genreService.GetGenresOfMediaEntry(mediaEntryId);
            string mediaType = "";
            if(mediaEntry is Movie)
            {
                mediaType = "Movie";
            }
            else if(mediaEntry is Series)
            {
                mediaType = "Series";
            }
            else if(mediaEntry is Game)
            {
                mediaType = "Game";
            }
            MediaEntryDTO mediaEntryDTO = new MediaEntryDTO(mediaEntry.Title, mediaEntry.Description,
                mediaType, mediaEntry.ReleaseYear, mediaEntry.AgeRestriction, genres);
            return JsonSerializer.Serialize(mediaEntryDTO);
        }
    }
}
