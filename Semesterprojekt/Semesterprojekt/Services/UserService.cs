using Semesterprojekt.Controllers;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Services
{
    public class UserService
    {
        UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void RegisterUser(string username, string password)
        {
            if(userRepository.UserExists(username))
            {
                throw new UserAlreadyExistsException("User with name " + username + " already exists!");
            }
            userRepository.SaveUser(username, password);
        }

        public void LoginUser(string username, string password)
        {
            if (!userRepository.UserExists(username))
            {
                throw new InvalidCredentialException("Incorrect Username or Password!");
            }
            userRepository.ValidateUser(username, password);
        }

        public User? GetUserById(int id)
        {
            return userRepository.GetUserById(id);
        }
    }
}
