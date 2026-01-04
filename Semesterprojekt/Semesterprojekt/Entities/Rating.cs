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
        private int _creator;
        private int _ofMediaEntry;
        private int _starRating;
        private string _comment;
        private DateTime? _createdAt;
        private HashSet<int> _likedBy;
        private bool _confirmedByAuthor;

        public int RatingId
        {
            get => _ratingId;
        }

        public int Creator
        {
            get => _creator;
        }

        public int OfMediaEntry
        {
            get => _ofMediaEntry;
        }

        public int StarRating
        {
            get => _starRating;
            set => _starRating = value;
        }

        public string Comment
        {
            get => _comment;
            set => _comment = value;
        }

        public DateTime? CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
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

        public Rating(int ratingId, int creator = -1, int ofMediaEntry = -1, int starRating = -1, string comment = "", 
            DateTime? createdAt = null, bool confirmedByAuthor = false)
        {
            _ratingId = ratingId;
            _creator = creator;
            _ofMediaEntry = ofMediaEntry;
            _starRating = starRating;
            _comment = comment;
            _createdAt = createdAt;
            _likedBy = new HashSet<int>();
            _confirmedByAuthor = confirmedByAuthor;
        }
    }
}
