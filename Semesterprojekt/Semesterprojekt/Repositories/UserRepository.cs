using Npgsql;
using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.General;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Semesterprojekt.Repositories
{
    public class UserRepository
    {
        private DatabaseConnector databaseConnector;

        public UserRepository(DatabaseConnector databaseConnector)
        {
            this.databaseConnector = databaseConnector;
        }

        public bool UserExists(string username)
        {
            bool userExists = false;
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT user_id FROM users WHERE username = @username";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userExists = true;
                        }
                    }
                }
                connection.Close();
            }
            return userExists;
        }

        public void SaveUser(string username, string password)
        {
            string saltString;
            string hashedPassword = PasswordHasher.HashPassword(password, out saltString);
            string sql = "INSERT INTO users (username, password, salt_string) VALUES (@username, @password, @salt_string)";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", hashedPassword);
                    command.Parameters.AddWithValue("salt_string", saltString);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void ValidateUser(string username, string password)
        {
            string savedSaltString = "";
            string savedHasedPassword = "";
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT password, salt_string FROM users WHERE username = @username";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            savedHasedPassword = reader.GetString(0);
                            savedSaltString = reader.GetString(1);
                        }
                    }
                }
                connection.Close();
            }

            string newHasedPassword = PasswordHasher.HashPassword(password, savedSaltString);
            if(!savedHasedPassword.Equals(newHasedPassword))
            {
                throw new InvalidCredentialException("Incorrect Username or Password!");
            }
        }

        public User? GetUserById(int id)
        {
            bool userFound = false;
            User user = new User(id);
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT username, email, favorite_genre FROM users WHERE user_id = @user_id";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("user_id", id);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userFound = true;
                            user.Username = reader.GetString(0);
                            user.Email =  reader.IsDBNull(1) ? "" : reader.GetString(1);
                            user.FavoriteGenre = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                        }
                    }
                }
                connection.Close();
            }
            if (userFound)
            {
                return user;
            }
            return null;
        }

        public User? GetUserByUsername(string username)
        {
            User user = null;
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT user_id, email, favorite_genre FROM users WHERE username = @username";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = new User(reader.GetInt32(0));
                            user.Username = username;
                            user.Email = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            user.FavoriteGenre = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);
                        }
                    }
                }
                connection.Close();
            }
            return user;
        }

        public void UpdateProfile(int userId, string email, int genreId)
        {
            string sql = "UPDATE users SET email = @email, favorite_genre = @favorite_genre WHERE user_id = @user_id";

            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.AddWithValue("favorite_genre", genreId);
                    command.Parameters.AddWithValue("user_id", userId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public List<LeaderboardEntryDTO> GetLeaderboard()
        {
            List<LeaderboardEntryDTO> leaderboardEntries = new List<LeaderboardEntryDTO>();
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT u.username, COUNT(r.creator) " +
                                    "FROM ratings r join users u ON r.creator = u.user_id " +
                                    "WHERE r.confirmed_by_author = true " +
                                    "GROUP BY u.username " +
                                    "ORDER BY COUNT(r.creator) DESC";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {                   
                            string username = reader.GetString(0);
                            int ratingCount = reader.GetInt32(1);
                            leaderboardEntries.Add(new LeaderboardEntryDTO(username, ratingCount));
                        }
                    }
                }
                connection.Close();
            }
            return leaderboardEntries;
        }
    }
}
