using Semesterprojekt.DTOs;
using Semesterprojekt.Exceptions;
using Semesterprojekt.General;
using Semesterprojekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Semesterprojekt.Controllers
{
    public class AuthController
    {
        public AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        public string Login(string requestBody)
        {
            UserInfoDTO userInfo = JsonSerializer.Deserialize<UserInfoDTO>(requestBody);
            if (userInfo == null || userInfo.username == null || userInfo.password == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            authService.LoginUser(userInfo.username, userInfo.password);
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
            authService.RegisterUser(userInfo.username, userInfo.password);
            JwtDTO jwtDTO = new JwtDTO(HttpUtility.GenerateJwtToken(userInfo.username));
            return JsonSerializer.Serialize(jwtDTO);
        }
    }
}
