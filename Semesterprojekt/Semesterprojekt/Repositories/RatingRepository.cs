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

        public void UpdateRating(Rating rating)
        {
            // TODO
        }
    }
}
