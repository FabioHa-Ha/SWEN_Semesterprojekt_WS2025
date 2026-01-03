using Npgsql;
using Semesterprojekt.Entities;
using Semesterprojekt.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

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
    }
}
