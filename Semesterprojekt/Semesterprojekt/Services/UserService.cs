using Semesterprojekt.Controllers;
using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.General;
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
        GenreService genreService;

        public UserService(UserRepository userRepository, GenreService genreService)
        {           
            this.userRepository = userRepository;
            this.genreService = genreService;
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

        public User GetValidUser(string token, int userId)
        {
            User? user = GetUserById(userId);
            if (user == null)
            {
                throw new InvalidUserException("Unkonw Id!");
            }
            if (!HttpUtility.ValidateJwtToken(token, user.Username))
            {
                throw new AuthenticationException("Invalid Token!");
            }
            return user;
        }

        public User? GetUserById(int id)
        {
            return userRepository.GetUserById(id);
        }

        public User? GetUserByUsername(string username)
        {
            return userRepository.GetUserByUsername(username);
        }

        public void UpdateProfile(User user, ProfileDTO profileDTO)
        {
            Genre genre = genreService.GetOrCreateGenre(profileDTO.favoriteGenre);
            userRepository.UpdateProfile(user.UserId, profileDTO.email, genre.GenreId);
        }
    }
}
