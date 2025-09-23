using System.Linq;

namespace Semesterprojekt.Entities
{
    public abstract class MediaEntry
    {
        protected int _mediaEntryId;
        protected string _title;
        protected string _description;
        protected int _releaseYear;
        protected HashSet<string> _genres;
        protected int _ageRestriction;
        protected int _creator;
        protected int creator;
        protected HashSet<int> _ratings;

        public int MediaEntryId
        {
            get => _mediaEntryId;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public int ReleaseYear
        {
            get => _releaseYear;
            set => _releaseYear = value;
        }
        public HashSet<string> Genres
        {
            get => _genres;
        }
        public int AgeRestriction
        {
            get => _ageRestriction;
            set => _ageRestriction = value;
        }
        public int Creator
        {
            get => _creator;
        }

        public HashSet<int> Ratings
        {
            get => _ratings;
        }

        public double AverageScore
        {
            get
            {
                if(Ratings.Count > 0)
                {
                    return Ratings.Average();
                }
                return 0;
            }
        }


        public MediaEntry(int mediaEntryId, string title, string description, int releaseYear, string genre,
            int ageRestriction, int creator)
        {
            _mediaEntryId = mediaEntryId;   // TODO: ID generieren lassen
            _title = title;
            _description = description;
            _releaseYear = releaseYear;
            _genres = new HashSet<string>();
            _genres.Add(genre);
            _ageRestriction = ageRestriction;
            _creator = creator;
            _ratings = new HashSet<int>();
        }
    }
}
