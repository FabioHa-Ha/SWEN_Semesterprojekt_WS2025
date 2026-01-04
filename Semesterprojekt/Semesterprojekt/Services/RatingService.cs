using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.PersistenceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.BusinessLayer
{
    public class RatingService
    {
        RatingRepository ratingRepository;

        public RatingService(RatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        public Rating? GetRating(int id)
        {
            return ratingRepository.GetRating(id);
        }

        public void CreateRating(MediaEntry mediaEntry, int userId, RatingDTO ratingDTO)
        {
            Rating? rating = ratingRepository.GetRating(userId, mediaEntry.MediaEntryId);
            if (rating != null)
            {
                throw new InvalidAccessException("You already rated this entry!");
            }
            if (mediaEntry.Creator == userId)
            {
                throw new InvalidAccessException("You cannot rate on your own entry!");
            }
            if (ratingDTO.stars < 1 || ratingDTO.stars > 5)
            {
                throw new InvalidStarRatingExcption("Invalid star rating!");
            }
            ratingRepository.CreateRating(mediaEntry.MediaEntryId, userId, ratingDTO);
        }

        public void LikeRating(int userId, Rating rating)
        {
            if(userId == rating.Creator)
            {
                throw new InvalidAccessException("You cannot like on your own rating!");
            }
            bool likeExists = ratingRepository.IsRatingLiked(userId, rating.RatingId);
            if (likeExists)
            {
                throw new InvalidAccessException("You already liked this rating!");
            }
            ratingRepository.LikeRating(userId, rating.RatingId);
        }

        public void UpdateRating(int userId, Rating rating, RatingDTO ratingDTO)
        {
            if (userId != rating.Creator)
            {
                throw new InvalidAccessException("You only update your own ratings!");
            }
            ratingRepository.UpdateRating(rating.RatingId, ratingDTO);
        }

        public void ConfirmRating(int userId, Rating rating)
        {
            if (userId != rating.Creator)
            {
                throw new InvalidAccessException("You only confrim your own ratings!");
            }
            if (rating.ConfirmedByAuthor)
            {
                throw new InvalidAccessException("You already confrimed this ratings!");
            }
            ratingRepository.ConfirmRating(rating.RatingId);
        }

        public void DeleteRating(int userId, Rating rating)
        {
            if (userId != rating.Creator)
            {
                throw new InvalidAccessException("You only delete your own ratings!");
            }
            ratingRepository.DeleteRating(rating.RatingId);
        }
    }
}
