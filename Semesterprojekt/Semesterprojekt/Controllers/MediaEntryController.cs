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

        public string GetAllMediaEntries(string token)
        {
            User user = userService.ValidateTokenAndGetUser(token);
            List<MediaEntry> mediaEntries = mediaEntryService.GetAllMediaEntries();
            string[] genres;
            string mediaType;
            List<MediaEntryDTO> mediaEntryDTOs = new List<MediaEntryDTO>();
            foreach (MediaEntry mediaEntry in mediaEntries)
            {
                MediaEntryDTO mediaEntryDTO = mediaEntryService.GetMediaEntryView(user.UserId, mediaEntry);
                mediaEntryDTOs.Add(mediaEntryDTO);
            }
            MediaEntriesDTO mediaEntiresDTO = new MediaEntriesDTO(mediaEntryDTOs.ToArray());
            return JsonSerializer.Serialize(mediaEntiresDTO);
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
