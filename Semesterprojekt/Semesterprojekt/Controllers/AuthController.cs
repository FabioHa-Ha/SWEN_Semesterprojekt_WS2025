using Semesterprojekt.DTOs;
using Semesterprojekt.Exceptions;
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
        public static async Task<HttpListenerResponse> Login(string requestBody, HttpListenerResponse response)
        {
            UserInfoDTO userInfo = JsonSerializer.Deserialize<UserInfoDTO>(requestBody);
            if (userInfo == null || userInfo.username == null || userInfo.password == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            return await HttpUtility.WriteJsonToResponse(response, JsonSerializer.Serialize(userInfo));
        }

        public static async Task<HttpListenerResponse> Register(string requestBody, HttpListenerResponse response)
        {
            UserInfoDTO userInfo = JsonSerializer.Deserialize<UserInfoDTO>(requestBody);
            if (userInfo == null || userInfo.username == null || userInfo.password == null)
            {
                throw new InvalidRequestBodyException("Invalid Request!");
            }
            return await HttpUtility.WriteJsonToResponse(response, JsonSerializer.Serialize(userInfo));
        }
    }
}
