using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt
{
    class Rating
    {
        private int _ratingId;
        private int _user;
        private int _mediaEntry;
        private int _starRating;
        private string? _comment;
        private DateTime _createdAt;
        private HashSet<int> _likedBy;
        private bool _confirmedByAuthor;

        public int RatingId
        {
            get => _ratingId;
        }

        public int SpecificUser
        {
            get => _user;
        }

        public int MediaEntry
        {
            get => _mediaEntry;
        }

        public int StarRating
        {
            get => _starRating;
            set {
                if(value < 1 || value > 5)
                {
                    throw new ArgumentException("Star rating has to be between 1 and 5!");
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

        Rating(int ratingId, int user, int mediaEntry, int starRating, string comment)
        {
            _ratingId = ratingId;   // TODO: ID generieren lassen
            _user = user;
            _mediaEntry = mediaEntry;
            StarRating = starRating;
            _comment = comment;
            _createdAt = DateTime.UtcNow;
            _likedBy = new HashSet<int>();
            _confirmedByAuthor = false;
        }
    }
}
