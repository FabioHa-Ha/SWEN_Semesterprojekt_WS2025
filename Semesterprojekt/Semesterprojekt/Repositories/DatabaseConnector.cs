using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Semesterprojekt.Repositories
{
    public class DatabaseConnector
    {
        private string connectionString;

        public DatabaseConnector()
        {
            connectionString = "Server=localhost; Port=5432; User Id=adminUser; Password=password; Database=testDatabase";
        }

        public NpgsqlConnection getConnection() 
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
