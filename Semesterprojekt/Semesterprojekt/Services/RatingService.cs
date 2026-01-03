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
    internal class RatingService
    {
        public void AddRating(MediaEntry mediaEntry, User user, int starRating, string comment)
        {
            int newRatingId = 1;
            Rating rating = new Rating(newRatingId, user.UserId, mediaEntry.MediaEntryId, starRating, comment);
            //mediaEntry.Ratings.Add(rating.RatingId);
            RatingRepository.CreateRating(rating);
            // TODO: Entity Repository Update
        }

        public void LikeRating(User user, Rating rating)
        {
            if (rating.Creator != user.UserId)
            {
                rating.LikedBy.Add(user.UserId);    // TODO: What happens when a user already liked a rating?
                RatingRepository.UpdateRating(rating);
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
                RatingRepository.UpdateRating(rating);
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
                RatingRepository.UpdateRating(rating);
            }
            else
            {
                throw new InvalidUserException("Only the rating creator can confirm a rating!");
            }
        }
    }
}
