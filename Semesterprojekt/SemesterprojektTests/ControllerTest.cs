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
        private AuthController authController;

        #region AuthController Test
        [TestInitialize]
        public void Setup()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();

            UserRepository userRepository = new UserRepository(databaseConnector);

            AuthService authService = new AuthService(userRepository);

            authController = new AuthController(authService);
        }

        [TestMethod]
        public void RegisterTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserRegister";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            string responseString = authController.Register(requestString);
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
            string responseString = authController.Register(requestString);

            Assert.ThrowsException<UserAlreadyExistsException>(() => authController.Register(requestString));
        }

        [TestMethod]
        public void LoginTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserLogin";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            authController.Register(requestString);

            string responseString = authController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);
        }

        [TestMethod]
        public void LoginFailTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserLoginFail";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            authController.Register(requestString);

            user.password = "password1";
            requestString = JsonSerializer.Serialize(user);
            Assert.ThrowsException<InvalidCredentialException>(() => authController.Login(requestString));
        }
        #endregion
    }
}
