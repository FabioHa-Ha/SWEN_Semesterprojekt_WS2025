using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Semesterprojekt.Controllers;
using Semesterprojekt.DTOs;
using Semesterprojekt.Exceptions;

namespace SemesterprojektTests
{
    [TestClass]
    public sealed class ControllerTest
    {
        #region AuthController Test
        [TestMethod]
        public void RegisterTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserRegister";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            string responseString = AuthController.Register(requestString);
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
            string responseString = AuthController.Register(requestString);

            Assert.ThrowsException<UserAlreadyExistsException>(() => AuthController.Register(requestString));
        }

        [TestMethod]
        public void LoginTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserLogin";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            AuthController.Register(requestString);

            string responseString = AuthController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);
        }

        [TestMethod]
        public void LoginFailTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserLoginFail";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            AuthController.Register(requestString);

            user.password = "password1";
            requestString = JsonSerializer.Serialize(user);
            Assert.ThrowsException<InvalidCredentialException>(() => AuthController.Login(requestString));
        }
        #endregion
    }
}
