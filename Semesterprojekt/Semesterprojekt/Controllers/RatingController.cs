using Semesterprojekt.BusinessLayer;
using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.General;
using Semesterprojekt.PersistenceLayer;
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

        public void CreateRating(string token, string mediaEntryIdString, string requestBody)
        {
            int mediaEntryId = Int32.Parse(mediaEntryIdString);
            string username = HttpUtility.ValidateJwtToken(token);
            if (username == "")
            {
                throw new InvalidCredentialException("Invalid Token!");
            }
            User user = userService.GetUserByUsername(username);
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
            string username = HttpUtility.ValidateJwtToken(token);
            if (username == "")
            {
                throw new InvalidCredentialException("Invalid Token!");
            }
            User user = userService.GetUserByUsername(username);
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
            string username = HttpUtility.ValidateJwtToken(token);
            if (username == "")
            {
                throw new InvalidCredentialException("Invalid Token!");
            }
            User user = userService.GetUserByUsername(username);
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
    }
}
