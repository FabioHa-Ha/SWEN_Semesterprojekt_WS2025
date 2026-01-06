using Semesterprojekt.Controllers;
using Semesterprojekt.DTOs;
using Semesterprojekt.Exceptions;
using Semesterprojekt.Repositories;
using Semesterprojekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SemesterprojektTests
{
    [TestClass]
    public sealed class UserControllerTest
    {
        private UserController userController;
        private UserRepository userRepository;

        #region UserController Test
        [TestInitialize]
        public void Setup()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();

            userRepository = new UserRepository(databaseConnector);
            GenreRepository genreRepository = new GenreRepository(databaseConnector);

            GenreService genreService = new GenreService(genreRepository);
            UserService userService = new UserService(userRepository, genreService);

            userController = new UserController(userService, genreService);
        }

        [TestMethod]
        public void RegisterTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserRegister";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            string responseString = userController.Register(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            Assert.IsNotNull(jwtDTO.token);

            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void RegisterFailTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserRegisterFail";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            string responseString = userController.Register(requestString);

            Assert.ThrowsException<UserAlreadyExistsException>(() => userController.Register(requestString));

            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void LoginTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserLogin";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void LoginFailTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserLoginFail";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            user.password = "password1";
            requestString = JsonSerializer.Serialize(user);
            Assert.ThrowsException<InvalidCredentialException>(() => userController.Login(requestString));

            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void GetProfileTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserGetProfile";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            responseString = userController.GetProfile(jwtDTO.token, userId.ToString());
            ProfileDTO profileDTO = JsonSerializer.Deserialize<ProfileDTO>(responseString);

            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void UpdateProfileTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserUpdateProfile";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            ProfileDTO profileDTO = new ProfileDTO("test@email.com", "Action");
            requestString = JsonSerializer.Serialize(profileDTO);
            userController.UpdateProfile(jwtDTO.token, userId.ToString(), requestString);

            responseString = userController.GetProfile(jwtDTO.token, userId.ToString());
            ProfileDTO getProfileDTO = JsonSerializer.Deserialize<ProfileDTO>(responseString);

            Assert.AreEqual(profileDTO.email, getProfileDTO.email);
            Assert.AreEqual(profileDTO.favoriteGenre, getProfileDTO.favoriteGenre);

            userRepository.DeleteUser(user.username);
        }
        #endregion
    }
}
