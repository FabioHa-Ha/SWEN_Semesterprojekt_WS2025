using Npgsql;
using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Repositories
{
    public class MediaEntryRepository
    {
        private DatabaseConnector databaseConnector;

        public MediaEntryRepository(DatabaseConnector databaseConnector)
        {
            this.databaseConnector = databaseConnector;
        }

        public MediaEntry? GetMediaEntry(int id)
        {
            MediaEntry? mediaEntry = null;
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT media_type, title, description, release_year, age_restriction, creator " +
                                    "FROM media_entries " +
                                    "WHERE media_entry_id = @media_entry_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("media_entry_id", id);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string mediaType = reader.GetString(0);
                            switch (mediaType)
                            {
                                case "Movie":
                                    mediaEntry = new Movie(id);
                                    break;
                                case "Series":
                                    mediaEntry = new Series(id);
                                    break;
                                case "Game":
                                    mediaEntry = new Game(id);
                                    break;
                            }
                            mediaEntry.Title = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            mediaEntry.Description = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            mediaEntry.ReleaseYear = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);
                            mediaEntry.AgeRestriction = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            mediaEntry.Creator = reader.GetInt32(5);
                        }
                    }
                }
                connection.Close();
            }
            return mediaEntry;
        }

        public List<MediaEntry> SearchMediaEntries(string title, string genre, string mediaType, string releaseYear,
            string ageRestriction)
        {
            List<MediaEntry> mediaEntries = new List<MediaEntry>();
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT DISTINCT m.media_entry_id, m.media_type, m.title, m.description, " +
                                        "m.release_year, m.age_restriction, m.creator " +
                                    "FROM media_entries m " +
                                        "JOIN media_entries_genres mg ON m.media_entry_id = mg.media_entry_id " +
                                        "JOIN genres g ON g.genre_id = mg.genre_id " +
                                    "WHERE (@title IS NULL OR m.title LIKE '%' || @title || '%') " +
                                        "AND (CAST(@genre AS VARCHAR) IS NULL OR g.name = @genre) " +
                                        "AND (CAST(@media_type AS VARCHAR) IS NULL OR m.media_type = @media_type) " +
                                        "AND (CAST(@release_year AS INT) IS NULL OR m.release_year = @release_year) " +
                                        "AND (CAST(@age_restriction AS INT) IS NULL OR m.age_restriction = @age_restriction)";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("title", title);
                    command.Parameters.AddWithValue("genre", genre.Equals("") ? DBNull.Value : genre);
                    command.Parameters.AddWithValue("media_type", mediaType.Equals("") ? DBNull.Value : mediaType);
                    command.Parameters.AddWithValue("release_year", releaseYear.Equals("") ? DBNull.Value : Int32.Parse(releaseYear));
                    command.Parameters.AddWithValue("age_restriction", ageRestriction.Equals("") ? DBNull.Value : Int32.Parse(ageRestriction));
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MediaEntry mediaEntry = null;
                            int id = reader.GetInt32(0);
                            string foudnMediaType = reader.GetString(1);
                            switch (foudnMediaType)
                            {
                                case "Movie":
                                    mediaEntry = new Movie(id);
                                    break;
                                case "Series":
                                    mediaEntry = new Series(id);
                                    break;
                                case "Game":
                                    mediaEntry = new Game(id);
                                    break;
                            }
                            mediaEntry.Title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            mediaEntry.Description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            mediaEntry.ReleaseYear = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            mediaEntry.AgeRestriction = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                            mediaEntry.Creator = reader.GetInt32(6);
                            mediaEntries.Add(mediaEntry);
                        }
                    }
                }
                connection.Close();
            }
            return mediaEntries;
        }

        public double GetAverageScore(int mediaEntryId)
        {
            double result = -1;
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT AVG(r.star_rating) " +
                                    "FROM media_entries m JOIN ratings r ON m.media_entry_id = r.of_media_entry " +
                                    "WHERE m.media_entry_id = @media_entry_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("media_entry_id", mediaEntryId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            result = reader.IsDBNull(0) ? -1 : reader.GetDouble(0);
                        }
                    }
                }
                connection.Close();
            }
            return result;
        }

        public List<MediaEntry> GetFavorites(int userId)
        {
            List<MediaEntry> mediaEntries = new List<MediaEntry>();
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT m.media_entry_id, m.media_type, m.title, m.description, " +
                                        "m.release_year, m.age_restriction " +
                                    "FROM favorite_media_entries f JOIN media_entries m ON f.media_entry_id = m.media_entry_id " +
                                    "WHERE f.user_id = @user_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("user_id", userId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MediaEntry mediaEntry = null;
                            int id = reader.GetInt32(0);
                            string mediaType = reader.GetString(1);
                            switch (mediaType)
                            {
                                case "Movie":
                                    mediaEntry = new Movie(id);
                                    break;
                                case "Series":
                                    mediaEntry = new Series(id);
                                    break;
                                case "Game":
                                    mediaEntry = new Game(id);
                                    break;
                            }
                            mediaEntry.Title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            mediaEntry.Description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            mediaEntry.ReleaseYear = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            mediaEntry.AgeRestriction = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                            mediaEntries.Add(mediaEntry);
                        }
                    }
                }
                connection.Close();
            }
            return mediaEntries;
        }

        public List<TypeCountDTO> GetPositiveGenreRatingCounts(int userId)
        {
            List<TypeCountDTO> typeCountDTOs = new List<TypeCountDTO>();
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT g.name, COUNT(*) " +
                                    "FROM media_entries m " +
                                        "JOIN ratings r ON m.media_entry_id = r.of_media_entry " +
                                        "JOIN media_entries_genres mg ON m.media_entry_id = mg.media_entry_id " +
                                        "JOIN genres g ON g.genre_id = mg.genre_id " +
                                    "WHERE r.creator = @user_id " +
                                        "AND r.star_rating >= 3 " +
                                    "GROUP BY g.name " +
                                    "ORDER BY COUNT(*)";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("user_id", userId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            int count = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            typeCountDTOs.Add(new TypeCountDTO(name, count));
                        }
                    }
                }
                connection.Close();
            }
            return typeCountDTOs;
        }

        public List<TypeCountDTO> GetPositiveMediaTypeRatingCounts(int userId)
        {
            List<TypeCountDTO> typeCountDTOs = new List<TypeCountDTO>();
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT mt.media_type_name, COALESCE(sub.count, 0)" +
                                    "FROM media_types mt " +
                                        "LEFT JOIN (SELECT m.media_type AS media_type, COUNT(*) AS count " +
                                            "FROM media_entries m " +
                                                "JOIN ratings r ON m.media_entry_id = r.of_media_entry " +
                                            "WHERE r.creator = @user_id " +
                                                "AND r.star_rating >= 3 " +
                                            "GROUP BY m.media_type) sub ON mt.media_type_name = sub.media_type " +
                                    "ORDER BY sub.count";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("user_id", userId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.IsDBNull(0) ? "" : reader.GetString(0);
                            int count = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            typeCountDTOs.Add(new TypeCountDTO(name, count));
                        }
                    }
                }
                connection.Close();
            }
            return typeCountDTOs;
        }

        public List<MediaEntry> GetNewMediaBasedOnGenre(int userId, string genre)
        {
            List<MediaEntry> mediaEntries = new List<MediaEntry>();
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT m.media_entry_id, m.media_type, m.title, m.description, " +
                                        "m.release_year, m.age_restriction, m.creator " +
                                    "FROM media_entries m " +
                                        "JOIN media_entries_genres mg ON m.media_entry_id = mg.media_entry_id " +
                                        "JOIN genres g ON g.genre_id = mg.genre_id " +
                                    "WHERE g.name = @genre " +
                                        "AND NOT EXISTS (SELECT 1 " +
                                                "FROM ratings r " +
                                                "WHERE r.creator = @user_id " +
                                                    "AND r.of_media_entry = m.media_entry_id) ";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("genre", genre);
                    command.Parameters.AddWithValue("user_id", userId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MediaEntry mediaEntry = null;
                            int id = reader.GetInt32(0);
                            string mediaType = reader.GetString(1);
                            switch (mediaType)
                            {
                                case "Movie":
                                    mediaEntry = new Movie(id);
                                    break;
                                case "Series":
                                    mediaEntry = new Series(id);
                                    break;
                                case "Game":
                                    mediaEntry = new Game(id);
                                    break;
                            }
                            mediaEntry.Title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            mediaEntry.Description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            mediaEntry.ReleaseYear = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            mediaEntry.AgeRestriction = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                            mediaEntry.Creator = reader.GetInt32(6);
                            mediaEntries.Add(mediaEntry);
                        }
                    }
                }
                connection.Close();
            }
            return mediaEntries;
        }


        public List<MediaEntry> GetNewMediaBasedOnMediaType(int userId, string mediaType)
        {
            List<MediaEntry> mediaEntries = new List<MediaEntry>();
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT m.media_entry_id, m.media_type, m.title, m.description, " +
                                        "m.release_year, m.age_restriction, m.creator " +
                                    "FROM media_entries m " +
                                    "WHERE m.media_type = @media_type " +
                                        "AND NOT EXISTS (SELECT 1 " +
                                                "FROM ratings r " +
                                                "WHERE r.creator = @user_id " +
                                                    "AND r.of_media_entry = m.media_entry_id)";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("media_type", mediaType);
                    command.Parameters.AddWithValue("user_id", userId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MediaEntry mediaEntry = null;
                            int id = reader.GetInt32(0);
                            string resultMediaType = reader.GetString(1);
                            switch (resultMediaType)
                            {
                                case "Movie":
                                    mediaEntry = new Movie(id);
                                    break;
                                case "Series":
                                    mediaEntry = new Series(id);
                                    break;
                                case "Game":
                                    mediaEntry = new Game(id);
                                    break;
                            }
                            mediaEntry.Title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            mediaEntry.Description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            mediaEntry.ReleaseYear = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            mediaEntry.AgeRestriction = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                            mediaEntry.Creator = reader.GetInt32(6);
                            mediaEntries.Add(mediaEntry);
                        }
                    }
                }
                connection.Close();
            }
            return mediaEntries;
        }

        public int CreateMediaEntry(MediaEntryDTO mediaEntryDTO, int userId)
        {
            int newId;
            string sql = "INSERT INTO media_entries (media_type, title, description, release_year, age_restriction, creator) " +
                "VALUES (@media_type, @title, @description, @release_year, @age_restriction, @creator) " +
                "RETURNING media_entry_id";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("media_type", mediaEntryDTO.mediaType);
                    command.Parameters.AddWithValue("title", mediaEntryDTO.title);
                    command.Parameters.AddWithValue("description", mediaEntryDTO.description);
                    command.Parameters.AddWithValue("release_year", mediaEntryDTO.releaseYear);
                    command.Parameters.AddWithValue("age_restriction", mediaEntryDTO.ageRestriction);
                    command.Parameters.AddWithValue("creator", userId);

                    newId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return newId;
        }

        public void AssignGenreToMediaEntry(int genreId, int mediaEntryId)
        {
            string sql = "INSERT INTO media_entries_genres (media_entry_id, genre_id) " +
                            "VALUES (@media_entry_id, @genre_id) ";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("media_entry_id", mediaEntryId);
                    command.Parameters.AddWithValue("genre_id", genreId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void DeleteMediaEntry(int mediaEntryId)
        {
            string sql = "DELETE FROM media_entries " +
                            "WHERE media_entry_id = @media_entry_id";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("media_entry_id", mediaEntryId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdateMediaEnty(int mediaEntryId, MediaEntryUpdateDTO mediaEntryDTO)
        {
            string sql = "UPDATE media_entries " +
                            "SET title = @title, description = @description, media_type = @media_type, " +
                            "release_year = @release_year, age_restriction = @age_restriction " +
                            "WHERE media_entry_id = @media_entry_id";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("title", mediaEntryDTO.title);
                    command.Parameters.AddWithValue("description", mediaEntryDTO.description);
                    command.Parameters.AddWithValue("media_type", mediaEntryDTO.mediaType);
                    command.Parameters.AddWithValue("release_year", mediaEntryDTO.releaseYear);
                    command.Parameters.AddWithValue("age_restriction", mediaEntryDTO.ageRestriction);
                    command.Parameters.AddWithValue("media_entry_id", mediaEntryId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void RemoveGenresFromMediaEntry(int mediaEntryId)
        {
            string sql = "DELETE FROM media_entries_genres " +
                            "WHERE media_entry_id = @media_entry_id";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("media_entry_id", mediaEntryId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public bool IsFavorite(int userId, int mediaEntryId)
        {
            bool found = false;
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT media_entry_id " +
                                    "FROM favorite_media_entries " +
                                    "WHERE user_id = @user_id AND media_entry_id = @media_entry_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("user_id", userId);
                    command.Parameters.AddWithValue("media_entry_id", mediaEntryId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            found = true;
                        }
                    }
                }
                connection.Close();
            }
            return found;
        }

        public void FavoriteMediaEntry(int userId, int mediaEntryId)
        {
            string sql = "INSERT INTO favorite_media_entries (user_id, media_entry_id) " +
                            "VALUES (@user_id, @media_entry_id) ";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("user_id", userId);
                    command.Parameters.AddWithValue("media_entry_id", mediaEntryId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }


        public void UnfavoriteMediaEntry(int userId, int mediaEntryId)
        {
            string sql = "DELETE FROM favorite_media_entries " +
                            "WHERE user_id = @user_id AND media_entry_id = @media_entry_id";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("user_id", userId);
                    command.Parameters.AddWithValue("media_entry_id", mediaEntryId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
