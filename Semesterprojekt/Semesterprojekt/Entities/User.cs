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
        private string _email;
        private int _favoriteGenre;

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

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public int FavoriteGenre
        {
            get => _favoriteGenre;
            set => _favoriteGenre = value;
        }

        public User(int userId, string username = "", string password = "", string email = "", int favoriteGenre = -1)
        {
            _userId = userId;
            _username = username;
            _password = password;
            _favoriteMediaEntries = new HashSet<int>();
            _email = email;
            _favoriteGenre = favoriteGenre;
        }
    }
}
