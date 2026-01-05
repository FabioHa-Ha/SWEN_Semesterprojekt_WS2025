using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.General;
using Semesterprojekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
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

        public string GetMediaEntry(string mediaEntryIdString)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            MediaEntry? mediaEntry = mediaEntryService.GetMediaEntry(mediaEntryId);
            if (mediaEntry == null)
            {
                throw new UnkownMediaEntryException("Invalid Id!");
            }
            string[] genres = genreService.GetGenresOfMediaEntry(mediaEntryId);
            string mediaType = GetMediaType(mediaEntry);
            double averageScore = mediaEntryService.GetAverageScore(mediaEntryId);
            MediaEntryDTO mediaEntryDTO = new MediaEntryDTO(mediaEntry.Title, mediaEntry.Description,
                mediaType, mediaEntry.ReleaseYear, mediaEntry.AgeRestriction, genres, averageScore);
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
                double averageScore = mediaEntryService.GetAverageScore(mediaEntry.MediaEntryId);
                MediaEntryDTO mediaEntryDTO = new MediaEntryDTO(mediaEntry.Title, mediaEntry.Description,
                    mediaType, mediaEntry.ReleaseYear, mediaEntry.AgeRestriction, genres, averageScore);
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

        public void DeleteMedia(string token, string mediaEntryIdString)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            string username = HttpUtility.ValidateJwtToken(token);
            if (username == "")
            {
                throw new InvalidCredentialException("Invalid Token!");
            }
            User user = userService.GetUserByUsername(username);
            mediaEntryService.DeleteMediaEntry(mediaEntryId, user.UserId);
        }

        public void UpdateMedia(string token, string mediaEntryIdString, string requestBody)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            string username = HttpUtility.ValidateJwtToken(token);
            if (username == "")
            {
                throw new InvalidCredentialException("Invalid Token!");
            }
            User user = userService.GetUserByUsername(username);
            MediaEntryUpdateDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryUpdateDTO>(requestBody);
            if (mediaEntryDTO == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            mediaEntryService.UpdateMediaEntry(user.UserId, mediaEntryId, mediaEntryDTO);
        }

        public void FavoriteMediaEntry(string token, string mediaEntryIdString)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            string username = HttpUtility.ValidateJwtToken(token);
            if (username == "")
            {
                throw new InvalidCredentialException("Invalid Token!");
            }
            User user = userService.GetUserByUsername(username);
            mediaEntryService.FavoriteMediaEntry(user.UserId, mediaEntryId);
        }


        public void UnfavoriteMediaEntry(string token, string mediaEntryIdString)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            string username = HttpUtility.ValidateJwtToken(token);
            if (username == "")
            {
                throw new InvalidCredentialException("Invalid Token!");
            }
            User user = userService.GetUserByUsername(username);
            mediaEntryService.UnfavoriteMediaEntry(user.UserId, mediaEntryId);
        }
    }
}
