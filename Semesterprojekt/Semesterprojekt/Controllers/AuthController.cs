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
    internal class AuthController
    {
        public static string Login(string requestBody)
        {
            UserInfoDTO userInfo = JsonSerializer.Deserialize<UserInfoDTO>(requestBody);
            if (userInfo == null || userInfo.username == null || userInfo.password == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            AuthService.LoginUser(userInfo.username, userInfo.password);
            JwtDTO jwtDTO = new JwtDTO(HttpUtility.GenerateJwtToken(userInfo.username));
            return JsonSerializer.Serialize(jwtDTO);
        }

        public static string Register(string requestBody)
        {
            UserInfoDTO userInfo = JsonSerializer.Deserialize<UserInfoDTO>(requestBody);
            if (userInfo == null || userInfo.username == null || userInfo.password == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            AuthService.RegisterUser(userInfo.username, userInfo.password);
            JwtDTO jwtDTO = new JwtDTO(HttpUtility.GenerateJwtToken(userInfo.username));
            return JsonSerializer.Serialize(jwtDTO);
        }
    }
}
