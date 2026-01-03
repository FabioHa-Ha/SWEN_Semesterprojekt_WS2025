using System.Linq;

namespace Semesterprojekt.Entities
{
    public abstract class MediaEntry
    {
        protected int _mediaEntryId;
        protected string _title;
        protected string _description;
        protected int _releaseYear;
        protected HashSet<int> _genres;
        protected int _ageRestriction;
        protected int _creator;

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
        public HashSet<int> Genres
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

        public double AverageScore
        {
            get
            {
                return 0;
            }
        }


        public MediaEntry(int mediaEntryId, string title, string description, int releaseYear,
            int ageRestriction, int creator)
        {
            _mediaEntryId = mediaEntryId;   // TODO: ID generieren lassen
            _title = title;
            _description = description;
            _releaseYear = releaseYear;
            _genres = new HashSet<int>();
            _ageRestriction = ageRestriction;
            _creator = creator;
        }
    }
}
