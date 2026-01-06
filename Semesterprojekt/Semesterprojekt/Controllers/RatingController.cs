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
    public class RatingController
    {
        RatingService ratingService;
        UserService userService;
        MediaEntryService mediaEntryService;

        public RatingController(RatingService ratingService, UserService userService, MediaEntryService mediaEntryService)
        { 
            this.ratingService = ratingService;
            this.userService = userService;
            this.mediaEntryService = mediaEntryService;
        }

        public string GetRatings(string token, string userIdString)
        {
            int userId = Int32.Parse(userIdString);
            User user = userService.GetValidUser(token, userId);
            List<Rating> ratings = ratingService.GetRatings(userId);
            List<RatingHistoryEntryDTO> ratingHistoryEntryDTOs = new List<RatingHistoryEntryDTO>();
            foreach (Rating rating in ratings)
            {
                string mediaTitle = mediaEntryService.GetMediaEntry(rating.OfMediaEntry).Title;
                ratingHistoryEntryDTOs.Add(new RatingHistoryEntryDTO(rating.Comment, 
                    rating.StarRating, (DateTime)rating.CreatedAt, mediaTitle, rating.ConfirmedByAuthor));
            }
            RatingHistoryDTO ratingHistoryDTO = new RatingHistoryDTO(ratingHistoryEntryDTOs.ToArray());
            return JsonSerializer.Serialize(ratingHistoryDTO);
        }

        public void CreateRating(string token, string mediaEntryIdString, string requestBody)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            User user = userService.ValidateTokenAndGetUser(token);
            MediaEntry? mediaEntry = mediaEntryService.GetMediaEntry(mediaEntryId);
            if (mediaEntry == null)
            {
                throw new UnkownMediaEntryException("Invalid id!");
            }
            RatingDTO ratingDTO = JsonSerializer.Deserialize<RatingDTO>(requestBody);
            if (ratingDTO == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            ratingService.CreateRating(mediaEntry, user.UserId, ratingDTO);
        }

        public void LikeRating(string token, string ratingIdString)
        {
            int ratingId = Int32.Parse(ratingIdString);
            User user = userService.ValidateTokenAndGetUser(token);
            Rating? rating = ratingService.GetRating(ratingId);
            if (rating == null)
            {
                throw new UnkownRatingException("Invalid id!");
            }
            ratingService.LikeRating(user.UserId, rating);
        }

        public void UpdateRating(string token, string ratingIdString, string requestBody)
        {
            int ratingId = Int32.Parse(ratingIdString);
            User user = userService.ValidateTokenAndGetUser(token);
            Rating? rating = ratingService.GetRating(ratingId);
            if (rating == null)
            {
                throw new UnkownRatingException("Invalid id!");
            }
            RatingDTO ratingDTO = JsonSerializer.Deserialize<RatingDTO>(requestBody);
            if (ratingDTO == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            ratingService.UpdateRating(user.UserId, rating, ratingDTO);
        }

        public void ConfirmRating(string token, string ratingIdString)
        {
            int ratingId = Int32.Parse(ratingIdString);
            User user = userService.ValidateTokenAndGetUser(token);
            Rating? rating = ratingService.GetRating(ratingId);
            if (rating == null)
            {
                throw new UnkownRatingException("Invalid id!");
            }
            ratingService.ConfirmRating(user.UserId, rating);
        }

        public void DeleteRating(string token, string ratingIdString)
        {
            int ratingId = Int32.Parse(ratingIdString);
            User user = userService.ValidateTokenAndGetUser(token);
            Rating? rating = ratingService.GetRating(ratingId);
            if (rating == null)
            {
                throw new UnkownRatingException("Invalid id!");
            }
            ratingService.DeleteRating(user.UserId, rating);
        }
    }
}
