using Npgsql;
using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.General;
using Semesterprojekt.Repositories;

namespace Semesterprojekt.PersistenceLayer
{
    public class RatingRepository
    {
        DatabaseConnector databaseConnector;

        public RatingRepository(DatabaseConnector databaseConnector)
        {
            this.databaseConnector = databaseConnector;
        }

        public Rating? GetRating(int ratingId)
        {
            Rating? rating = null;
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT of_media_entry, creator, star_rating, rating_comment, created_at, confirmed_by_author " +
                                    "FROM ratings " +
                                    "WHERE rating_id = @rating_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("rating_id", ratingId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rating = new Rating(ratingId);
                            rating.OfMediaEntry = reader.GetInt32(0);
                            rating.Creator = reader.GetInt32(1);
                            rating.StarRating = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                            rating.Comment = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            rating.CreatedAt = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                            rating.ConfirmedByAuthor = reader.GetBoolean(5);
                        }
                    }
                }
                connection.Close();
            }
            return rating;
        }

        public Rating? GetRating(int userId, int mediaRatingId)
        {
            Rating? rating = null;
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT rating_id, star_rating, rating_comment, created_at, confirmed_by_author " +
                                    "FROM ratings " +
                                    "WHERE of_media_entry = @media_entry_id AND creator = @user_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("media_entry_id", mediaRatingId);
                    command.Parameters.AddWithValue("user_id", userId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rating = new Rating(reader.GetInt32(0));
                            rating.StarRating = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                            rating.Comment = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            rating.CreatedAt = reader.IsDBNull(3) ? null : reader.GetDateTime(3);
                            rating.ConfirmedByAuthor = reader.GetBoolean(4);
                        }
                    }
                }
                connection.Close();
            }
            return rating;
        }

        public int CreateRating(int mediaEntryId, int userId, RatingDTO ratingDTO)
        {
            int newId;
            string sql = "INSERT INTO ratings (creator, of_media_entry, star_rating, rating_comment, confirmed_by_author) " +
                            "VALUES (@creator, @of_media_entry, @star_rating, @comment, false) " +
                            "RETURNING rating_id";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("creator", userId);
                    command.Parameters.AddWithValue("of_media_entry", mediaEntryId);
                    command.Parameters.AddWithValue("star_rating", ratingDTO.stars);
                    command.Parameters.AddWithValue("comment", ratingDTO.comment);

                    newId = (int)command.ExecuteScalar();
                }
                connection.Close();
            }
            return newId;
        }

        public bool IsRatingLiked(int userId, int ratingId)
        {
            bool found = false;
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT rating_id " +
                                    "FROM rating_likes " +
                                    "WHERE rating_id = @rating_id AND user_id = @user_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("rating_id", ratingId);
                    command.Parameters.AddWithValue("user_id", userId);
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

        public void LikeRating(int userId, int ratingId)
        {
            string sql = "INSERT INTO rating_likes (user_id, rating_id) " +
                            "VALUES (@user_id, @rating_id)";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("user_id", userId);
                    command.Parameters.AddWithValue("rating_id", ratingId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void UpdateRating(int ratingId, RatingDTO ratingDTO)
        {
            string sql = "UPDATE ratings " +
                            "SET star_rating = @star_rating, rating_comment = @comment " +
                            "WHERE rating_id = @rating_id";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("star_rating", ratingDTO.stars);
                    command.Parameters.AddWithValue("comment", ratingDTO.comment);
                    command.Parameters.AddWithValue("rating_id", ratingId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
