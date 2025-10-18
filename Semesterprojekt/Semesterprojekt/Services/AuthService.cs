using Semesterprojekt.Controllers;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Services
{
    internal class AuthService
    {
        public static void RegisterUser(string username, string password)
        {
            if(UserRepository.UserExists(username))
            {
                throw new UserAlreadyExistsException("User with name " + username + " already exists!");
            }
            User user = new User(UserRepository.GetNewId(), username, password);
            UserRepository.SaveUser(user);
        }
    }
}
