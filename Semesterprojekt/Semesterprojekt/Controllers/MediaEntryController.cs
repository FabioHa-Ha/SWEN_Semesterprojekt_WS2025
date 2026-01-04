using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.General;
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
        UserService userService;

        public MediaEntryController(MediaEntryService mediaEntryService, GenreService genreService, UserService userService)
        {
            this.mediaEntryService = mediaEntryService;
            this.genreService = genreService;
            this.userService = userService;
        }

        private string GetMediaType(MediaEntry mediaEntry)
        {
            string mediaType = "";
            if (mediaEntry is Movie)
            {
                mediaType = "Movie";
            }
            else if (mediaEntry is Series)
            {
                mediaType = "Series";
            }
            else if (mediaEntry is Game)
            {
                mediaType = "Game";
            }
            return mediaType;
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
            string mediaType = GetMediaType(mediaEntry);
            MediaEntryDTO mediaEntryDTO = new MediaEntryDTO(mediaEntry.Title, mediaEntry.Description,
                mediaType, mediaEntry.ReleaseYear, mediaEntry.AgeRestriction, genres);
            return JsonSerializer.Serialize(mediaEntryDTO);
        }

        public string GetAllMediaEntries()
        {
            List<MediaEntry> mediaEntries = mediaEntryService.GetAllMediaEntries();
            string[] genres;
            string mediaType;
            List<MediaEntryDTO> mediaEntryDTOs = new List<MediaEntryDTO>();
            foreach (MediaEntry mediaEntry in mediaEntries)
            {
                genres = genreService.GetGenresOfMediaEntry(mediaEntry.MediaEntryId);
                mediaType = GetMediaType(mediaEntry);
                MediaEntryDTO mediaEntryDTO = new MediaEntryDTO(mediaEntry.Title, mediaEntry.Description,
                    mediaType, mediaEntry.ReleaseYear, mediaEntry.AgeRestriction, genres);
                mediaEntryDTOs.Add(mediaEntryDTO);
            }
            MediaEntiresDTO mediaEntiresDTO = new MediaEntiresDTO(mediaEntryDTOs.ToArray());
            return JsonSerializer.Serialize(mediaEntiresDTO);
        }

        public void CreateMedia(string token, string requestBody)
        {
            MediaEntryDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(requestBody);
            if (mediaEntryDTO == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            string username = HttpUtility.ValidateJwtToken(token);
            User user = userService.GetUserByUsername(username);
            mediaEntryService.CreateMediaEntry(mediaEntryDTO, user.UserId);
        }
    }
}
