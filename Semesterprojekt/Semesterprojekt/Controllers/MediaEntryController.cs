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

        public string GetMediaEntry(string token, string mediaEntryIdString)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            User user = userService.ValidateTokenAndGetUser(token);
            MediaEntry? mediaEntry = mediaEntryService.GetMediaEntry(mediaEntryId);
            if (mediaEntry == null)
            {
                throw new UnkownMediaEntryException("Invalid Id!");
            }
            MediaEntryDTO mediaEntryDTO = mediaEntryService.GetMediaEntryView(user.UserId, mediaEntry);
            return JsonSerializer.Serialize(mediaEntryDTO);
        }

        public string SearchMediaEntries(string token, string url)
        {
            User user = userService.ValidateTokenAndGetUser(token);

            string title = "";
            string genre = "";
            string mediaType = "";
            string releaseYear = "";
            string ageRestriction = "";
            string rating = "";
            string sortBy = "";

            string[] urlParts = url.Split('?');
            if (urlParts.Length > 2) 
            {
                throw new InvalidRequestQueryException("Invalid Request Query!");
            }
            if (url.Contains("?"))
            {
                string[] queryParams = urlParts[1].Split('&');
                foreach (string queryParam in queryParams)
                {
                    string[] queryParts = queryParam.Split("=");
                    if (queryParts.Length != 2)
                    {
                        throw new InvalidRequestQueryException("Invalid Request Query!");
                    }
                    switch (queryParts[0])
                    {
                        case "title":
                            title = queryParts[1];
                            break;
                        case "genre":
                            genre = queryParts[1];
                            break;
                        case "mediaType":
                            mediaType = queryParts[1];
                            break;
                        case "releaseYear":
                            releaseYear = queryParts[1];
                            break;
                        case "ageRestriction":
                            ageRestriction = queryParts[1];
                            break;
                        case "rating":
                            rating = queryParts[1];
                            break;
                        case "sortBy":
                            sortBy = queryParts[1];
                            break;
                        default:
                            throw new InvalidRequestQueryException("Invalid Request Query!");
                    }
                }
            }

            List<MediaEntry> mediaEntries = mediaEntryService.SearchMediaEntries(title, genre, mediaType, releaseYear,
                ageRestriction);

            List<MediaEntryDTO> mediaEntryDTOs = mediaEntryService.FilterAndConvertMediaEntries(mediaEntries, user.UserId, rating, sortBy);

            MediaEntriesDTO mediaEntiresDTO = new MediaEntriesDTO(mediaEntryDTOs.ToArray());
            return JsonSerializer.Serialize(mediaEntiresDTO);
        }

        public string GetFavorites(string token, string userIdString)
        {
            int userId = Int32.Parse(userIdString);
            User user = userService.GetValidUser(token, userId);
            List<MediaEntry> mediaEntries = mediaEntryService.GetFavorites(userId);
            List<MediaEntryDTO> mediaEntryDTOs = new List<MediaEntryDTO>();
            foreach (MediaEntry mediaEntry in mediaEntries)
            {
                MediaEntryDTO mediaEntryDTO = mediaEntryService.GetMediaEntryView(user.UserId, mediaEntry);
                mediaEntryDTOs.Add(mediaEntryDTO);
            }
            MediaEntriesDTO mediaEntriesDTO = new MediaEntriesDTO(mediaEntryDTOs.ToArray());
            return JsonSerializer.Serialize(mediaEntriesDTO);
        }

        public void CreateMedia(string token, string requestBody)
        {
            MediaEntryDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(requestBody);
            if (mediaEntryDTO == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            User user = userService.ValidateTokenAndGetUser(token);
            mediaEntryService.CreateMediaEntry(mediaEntryDTO, user.UserId);
        }

        public void DeleteMedia(string token, string mediaEntryIdString)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            User user = userService.ValidateTokenAndGetUser(token);
            mediaEntryService.DeleteMediaEntry(mediaEntryId, user.UserId);
        }

        public void UpdateMedia(string token, string mediaEntryIdString, string requestBody)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            User user = userService.ValidateTokenAndGetUser(token);
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
            User user = userService.ValidateTokenAndGetUser(token);
            mediaEntryService.FavoriteMediaEntry(user.UserId, mediaEntryId);
        }


        public void UnfavoriteMediaEntry(string token, string mediaEntryIdString)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            User user = userService.ValidateTokenAndGetUser(token);
            mediaEntryService.UnfavoriteMediaEntry(user.UserId, mediaEntryId);
        }
    }
}
