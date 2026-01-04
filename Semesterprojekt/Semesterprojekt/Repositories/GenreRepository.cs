using Npgsql;
using Semesterprojekt.Entities;
using Semesterprojekt.General;
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

        public Genre? GetGenreByName(string name)
        {
            bool genreFound = false;
            Genre genre = new Genre(-1, name);
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT genre_id FROM genres WHERE name = @name";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("name", name);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            genreFound = true;
                            genre.GenreId = reader.GetInt32(0);
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

        public void CreateGenre(string name)
        {
            string sql = "INSERT INTO genres (name) VALUES (@name)";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("name", name);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
