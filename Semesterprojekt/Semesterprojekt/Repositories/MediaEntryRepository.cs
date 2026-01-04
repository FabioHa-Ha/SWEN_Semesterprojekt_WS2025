using Npgsql;
using Semesterprojekt.Entities;
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

        public List<MediaEntry> GetAllMediaEntries()
        {
            List<MediaEntry> mediaEntries = new List<MediaEntry>();
            NpgsqlConnection connection = databaseConnector.getConnection();
            using (connection)
            {
                connection.Open();
                string query = "SELECT media_entry_id, media_type, title, description, " +
                                        "release_year, age_restriction, creator " +
                                    "FROM media_entries";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
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
                            mediaEntry.Creator = reader.GetInt32(5);
                            mediaEntries.Add(mediaEntry);
                        }
                    }
                }
                connection.Close();
            }
            return mediaEntries;
        }
    }
}
