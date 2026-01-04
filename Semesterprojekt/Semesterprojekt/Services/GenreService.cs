using Semesterprojekt.Entities;
using Semesterprojekt.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Services
{
    public class GenreService
    {
        GenreRepository genreRepository;

        public GenreService(GenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public Genre? GetGenreById(int id)
        {
            return genreRepository.GetGenreById(id);
        }
    }
}
