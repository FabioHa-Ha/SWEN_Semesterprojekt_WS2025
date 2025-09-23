using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.BusinessLayer
{
    internal class RatingManager
    {
        public void AddRating(MediaEntry mediaEntry, User user, int starRating, string comment)
        {
            int newRatingId = 1;
            Rating rating = new Rating(newRatingId, user.UserId, mediaEntry.MediaEntryId, starRating, comment);
            mediaEntry.Ratings.Add(rating.RatingId);
            // TODO: Send to persistence layer
        }

        public void LikeRating(User user, Rating rating)
        {
            if (rating.UserId != user.UserId)
            {
                rating.LikedBy.Add(user.UserId);    // TODO: What happens when a user already liked a rating?
                // TODO: Send update to persistence layer
            }
            else
            {
                throw new InvalidUserException("You can only like other peoples ratings!");
            }
        }

        public void EditRating(User user, Rating rating, int newStarRating, string newComment)
        {
            if (rating.UserId == user.UserId)
            {
                rating.StarRating = newStarRating;
                rating.Comment = newComment;
                // TODO: Send update to persistence layer
            }
            else
            {
                throw new InvalidUserException("Only the rating creator can edit a rating!");
            }
        }

        public void ConfirmRating(Rating rating, User user)
        {
            if (rating.UserId == user.UserId) 
            { 
                rating.ConfirmedByAuthor = true;
                // TODO: Send update to persistence layer
            }
            else
            {
                throw new InvalidUserException("Only the rating creator can confirm a rating!");
            }
        }
    }
}
