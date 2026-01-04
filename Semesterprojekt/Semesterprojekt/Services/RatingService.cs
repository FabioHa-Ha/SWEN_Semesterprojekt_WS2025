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

        public void LikeRating(User user, Rating rating)
        {
            if (rating.Creator != user.UserId)
            {
                rating.LikedBy.Add(user.UserId);    // TODO: What happens when a user already liked a rating?
                ratingRepository.UpdateRating(rating);
            }
            else
            {
                throw new InvalidUserException("You can only like other peoples ratings!");
            }
        }

        public void EditRating(User user, Rating rating, int newStarRating, string newComment)
        {
            if (rating.Creator == user.UserId)
            {
                rating.StarRating = newStarRating;
                rating.Comment = newComment;
                ratingRepository.UpdateRating(rating);
            }
            else
            {
                throw new InvalidUserException("Only the rating creator can edit a rating!");
            }
        }

        public void ConfirmRating(Rating rating, User user)
        {
            if (rating.Creator == user.UserId) 
            { 
                rating.ConfirmedByAuthor = true;
                ratingRepository.UpdateRating(rating);
            }
            else
            {
                throw new InvalidUserException("Only the rating creator can confirm a rating!");
            }
        }
    }
}
