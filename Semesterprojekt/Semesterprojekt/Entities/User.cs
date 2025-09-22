using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Entities
{
    public class User
    {
        private int _userId;
        private string _username;
        private string _password;
        private HashSet<int> _favoriteMediaEntries;

        public int UserId
        {
            get => _userId; 
            set => _userId = value;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public HashSet<int> FavoriteMediaEntries
        {
            get => _favoriteMediaEntries;
        }

        public User(int userId, string username, string password)
        {
            _userId = userId;   // TODO: ID generieren lassen
            _username = username;
            _password = password;
            _favoriteMediaEntries = new HashSet<int>();
        }
    }
}
