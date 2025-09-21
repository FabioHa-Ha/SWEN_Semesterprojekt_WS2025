namespace Semesterprojekt
{
    abstract class MediaEntry
    {
        protected int _mediaEntryId;
        protected string _title;
        protected string _description;
        protected string _mediaType;
        protected int _releaseYear;
        protected HashSet<string> _genres;
        protected int _ageRestriction;
        protected int _creator;
        private int creator;

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
        public string MediaType
        {
            get => _mediaType;
            set => _mediaType = value;
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


        public MediaEntry(int mediaEntryId, string title, string description, string mediaType, int releaseYear, string genre,
            int ageRestriction, int creator)
        {
            _mediaEntryId = mediaEntryId;   // TODO: ID generieren lassen
            _title = title;
            _description = description;
            _mediaType = mediaType;
            _releaseYear = releaseYear;
            _genres = new HashSet<string>();
            _genres.Add(genre);
            _ageRestriction = ageRestriction;
            _creator = creator;
        }
    }
}
