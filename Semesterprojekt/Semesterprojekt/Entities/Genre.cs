using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Entities
{
    public class Genre
    {
        private int _genreId;
        private string _name;

        public int GenreId
        {
            get => _genreId;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public Genre(int genreId, string name)
        {
            _genreId = genreId;   // TODO: ID generieren lassen
            _name = name;
        }
    }
}
