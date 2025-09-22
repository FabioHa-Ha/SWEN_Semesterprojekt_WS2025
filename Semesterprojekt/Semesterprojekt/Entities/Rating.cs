using Semesterprojekt.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Entities
{
    public class Rating
    {
        private int _ratingId;
        private int _userId;
        private int _mediaEntryId;
        private int _starRating;
        private string? _comment;
        private DateTime _createdAt;
        private HashSet<int> _likedBy;
        private bool _confirmedByAuthor;

        public int RatingId
        {
            get => _ratingId;
        }

        public int UserId
        {
            get => _userId;
        }

        public int MediaEntryId
        {
            get => _mediaEntryId;
        }

        public int StarRating
        {
            get => _starRating;
            set {
                if(value < 1 || value > 5)
                {
                    throw new InvalidStarRatingExcption("Star rating has to be between 1 and 5!");
                }
                _starRating = value;
            }
        }

        public string? Comment
        {
            get => _comment;
            set => _comment = value;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
        }

        public HashSet<int> LikedBy
        {
            get => _likedBy;
        }

        public bool ConfirmedByAuthor
        {
            get => _confirmedByAuthor;
            set => _confirmedByAuthor = value;
        }

        public Rating(int ratingId, int userId, int mediaEntryId, int starRating, string comment)
        {
            _ratingId = ratingId;   // TODO: ID generieren lassen
            _userId = userId;
            _mediaEntryId = mediaEntryId;
            StarRating = starRating;
            _comment = comment;
            _createdAt = DateTime.UtcNow;
            _likedBy = new HashSet<int>();
            _confirmedByAuthor = false;
        }
    }
}
