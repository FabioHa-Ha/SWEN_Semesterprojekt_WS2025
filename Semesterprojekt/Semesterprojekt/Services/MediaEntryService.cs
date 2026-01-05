using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
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

        public void DeleteMediaEntry(int mediaEntryId, int userId)
        {
            MediaEntry mediaEntry = GetMediaEntry(mediaEntryId);
            if (mediaEntry == null) 
            {
                throw new UnkownMediaEntryException("Invalid id!");
            }
            if (mediaEntry.Creator != userId)
            {
                throw new UnauthorizedAccessException("Media Entries can only be deleted by its creator!");
            }
            mediaEntryRepository.DeleteMediaEntry(mediaEntryId);
        }

        public void UpdateMediaEntry(int userId, int mediaEntryId, MediaEntryDTO mediaEntryDTO)
        {
            MediaEntry mediaEntry = GetMediaEntry(mediaEntryId);
            if (mediaEntry == null)
            {
                throw new UnkownMediaEntryException("Invalid id!");
            }
            if (mediaEntry.Creator != userId)
            {
                throw new UnauthorizedAccessException("Media Entries can only be deleted by its creator!");
            }
            mediaEntryRepository.UpdateMediaEnty(mediaEntryId, mediaEntryDTO);
            mediaEntryRepository.RemoveGenresFromMediaEntry(mediaEntryId);
            foreach(string genreName in mediaEntryDTO.genres)
            {
                Genre genre = genreService.GetOrCreateGenre(genreName);
                mediaEntryRepository.AssignGenreToMediaEntry(genre.GenreId, mediaEntryId);
            }
        }

        public void FavoriteMediaEntry(int userId, int mediaEntryId)
        {
            MediaEntry mediaEntry = GetMediaEntry(mediaEntryId);
            if (mediaEntry == null)
            {
                throw new UnkownMediaEntryException("Invalid id!");
            }
            if (mediaEntry.Creator == userId)
            {
                throw new InvalidAccessException("You cannot mark your own Media Entries as a favorite!");
            }
            bool isFavorite = mediaEntryRepository.IsFavorite(userId, mediaEntryId);
            if (isFavorite)
            {
                throw new InvalidAccessException("You already makred this Media Entries as a favorite!");
            }
            mediaEntryRepository.FavoriteMediaEntry(userId, mediaEntryId);
        }

        public void UnfavoriteMediaEntry(int userId, int mediaEntryId)
        {
            MediaEntry mediaEntry = GetMediaEntry(mediaEntryId);
            if (mediaEntry == null)
            {
                throw new UnkownMediaEntryException("Invalid id!");
            }
            if (mediaEntry.Creator == userId)
            {
                throw new InvalidAccessException("You can only edit your own favorite Media Entries!");
            }
            bool isFavorite = mediaEntryRepository.IsFavorite(userId, mediaEntryId);
            if (!isFavorite)
            {
                throw new InvalidAccessException("You have not makred this Media Entries as a favorite!");
            }
            mediaEntryRepository.UnfavoriteMediaEntry(userId, mediaEntryId);
        }
    }
}
