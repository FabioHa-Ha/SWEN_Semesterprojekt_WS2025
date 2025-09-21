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

        Rating(int ratingId, int user, int mediaEntry)
        {
            _ratingId = ratingId;   // TODO: ID generieren lassen
            _user = user;
            _mediaEntry = mediaEntry;
        }

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
    }
}
