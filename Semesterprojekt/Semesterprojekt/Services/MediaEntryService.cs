using Semesterprojekt.BusinessLayer;
using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Services
{
    public class MediaEntryService
    {
        MediaEntryRepository mediaEntryRepository;
        GenreService genreService;
        RatingService ratingService;

        public MediaEntryService(MediaEntryRepository mediaEntryRepository, GenreService genreService,
            RatingService ratingService) 
        {
            this.mediaEntryRepository = mediaEntryRepository;
            this.genreService = genreService;
            this.ratingService = ratingService;
        }

        public MediaEntry? GetMediaEntry(int id)
        {
            return mediaEntryRepository.GetMediaEntry(id);
        }

        public List<MediaEntry> SearchMediaEntries(string title, string genre, string mediaType, string releaseYear,
            string ageRestriction)
        {
            return mediaEntryRepository.SearchMediaEntries(title, genre, mediaType, releaseYear,
                ageRestriction);
        }

        public List<MediaEntryDTO> FilterAndConvertMediaEntries(List<MediaEntry> mediaEntries, int userId, string rating, string sortBy)
        {
            List<MediaEntryDTO> mediaEntryDTOs = new List<MediaEntryDTO>();
            double ratingValue = 0;
            if (rating == "")
            {
                ratingValue = -1;
            }
            else
            {
                ratingValue = Double.Parse(rating);
            }
            foreach (MediaEntry mediaEntry in mediaEntries)
            {
                MediaEntryDTO mediaEntryDTO = GetMediaEntryView(userId, mediaEntry);
                if (mediaEntryDTO.averageScore >= ratingValue)
                {
                    mediaEntryDTOs.Add(mediaEntryDTO);
                }
            }
            switch (sortBy)
            {
                case "title":
                    mediaEntryDTOs = mediaEntryDTOs.OrderBy(o => o.title).ToList();
                    break;
                case "year":
                    mediaEntryDTOs = mediaEntryDTOs.OrderBy(o => o.releaseYear).ToList();
                    break;
                case "rating":
                    mediaEntryDTOs = mediaEntryDTOs.OrderBy(o => o.averageScore).ToList();
                    break;
                default:
                    mediaEntryDTOs = mediaEntryDTOs.OrderBy(o => o.mediaType).ToList();
                    break;
            }

            return mediaEntryDTOs;
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

        public MediaEntryDTO GetMediaEntryView(int userId, MediaEntry mediaEntry)
        {
            string[] genres = genreService.GetGenresOfMediaEntry(mediaEntry.MediaEntryId);
            string mediaType = GetMediaType(mediaEntry);
            double averageScore = GetAverageScore(mediaEntry.MediaEntryId);
            List<RatingViewDTO> ratings = ratingService.GetRatingViewsOfMediaEntry(userId, mediaEntry.MediaEntryId);
            return new MediaEntryDTO(mediaEntry.Title, mediaEntry.Description, mediaType, 
                mediaEntry.ReleaseYear, mediaEntry.AgeRestriction, genres, averageScore, ratings.ToArray());
        }

        public double GetAverageScore(int mediaEntryId)
        {
            return mediaEntryRepository.GetAverageScore(mediaEntryId);
        }

        public List<MediaEntry> GetFavorites(int userId)
        {
            return mediaEntryRepository.GetFavorites(userId);
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

        public void UpdateMediaEntry(int userId, int mediaEntryId, MediaEntryUpdateDTO mediaEntryDTO)
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
