using Npgsql;
using Semesterprojekt.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Repositories
{
    public class GenreRepository
    {
        private DatabaseConnector databaseConnector;

        public GenreRepository(DatabaseConnector databaseConnector)
        {
            this.databaseConnector = databaseConnector;
        }

        public Genre? GetGenreById(int id)
        {
            bool genreFound = false;
            Genre genre = new Genre(id);
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT name FROM genres WHERE genre_id = @genre_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("genre_id", id);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            genreFound = true;
                            genre.Name = reader.GetString(0);
                        }
                    }
                }
                connection.Close();
            }
            if (genreFound)
            {
                return genre;
            }
            return null;
        }
    }
}
