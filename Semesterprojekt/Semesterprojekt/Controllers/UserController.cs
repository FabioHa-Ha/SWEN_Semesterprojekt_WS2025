using Semesterprojekt.DTOs;
using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;
using Semesterprojekt.General;
using Semesterprojekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Semesterprojekt.Controllers
{
    public class UserController
    {
        public UserService userService;
        public GenreService genreService;

        public UserController(UserService userService, GenreService genreService)
        {
            this.userService = userService;
            this.genreService = genreService;
        }

        public string Login(string requestBody)
        {
            UserInfoDTO userInfo = JsonSerializer.Deserialize<UserInfoDTO>(requestBody);
            if (userInfo == null || userInfo.username == null || userInfo.password == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            userService.LoginUser(userInfo.username, userInfo.password);
            JwtDTO jwtDTO = new JwtDTO(HttpUtility.GenerateJwtToken(userInfo.username));
            return JsonSerializer.Serialize(jwtDTO);
        }

        public string Register(string requestBody)
        {
            UserInfoDTO userInfo = JsonSerializer.Deserialize<UserInfoDTO>(requestBody);
            if (userInfo == null || userInfo.username == null || userInfo.password == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            userService.RegisterUser(userInfo.username, userInfo.password);
            JwtDTO jwtDTO = new JwtDTO(HttpUtility.GenerateJwtToken(userInfo.username));
            return JsonSerializer.Serialize(jwtDTO);
        }

        public string GetProfile(string token, string userIdString)
        {
            int userId = Int32.Parse(userIdString);
            User? user = userService.GetUserById(userId);
            if(user == null)
            {
                throw new InvalidUserException("Unkonw Id!");
            }
            if(!HttpUtility.ValidateJwtToken(token, user.Username))
            {
                throw new AuthenticationException("Invalid Token!");
            }
            Genre? genre = genreService.GetGenreById(user.FavoriteGenre);
            string genreName = "";
            if (genre != null)
            {
                genreName = genre.Name;
            }
            ProfileDTO profileDTO = new ProfileDTO(user.Email, genreName);
            return JsonSerializer.Serialize(profileDTO);
        }
    }
}
