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
    public sealed class ControllerTest
    {
        private UserController userController;

        #region UserController Test
        [TestInitialize]
        public void Setup()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();

            UserRepository userRepository = new UserRepository(databaseConnector);
            GenreRepository genreRepository = new GenreRepository(databaseConnector);

            GenreService genreService = new GenreService(genreRepository);
            UserService userService = new UserService(userRepository, genreService);

            UserController userController = new UserController(userService, genreService);

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
        }
        #endregion
    }
}
