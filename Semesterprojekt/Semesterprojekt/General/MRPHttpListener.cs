using Semesterprojekt.Controllers;
using Semesterprojekt.DTOs;
using Semesterprojekt.Exceptions;
using Semesterprojekt.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Semesterprojekt.General
{
    public class MRPHttpListener
    {
        public static string NotFoundResponse(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseText = $"Error 404:\nThe requested page: {request.HttpMethod} {request.Url} is not available";
            ErrorDTO errorDTO = new ErrorDTO(responseText);
            return JsonSerializer.Serialize(errorDTO);
        }

        public static string ErrorResponse(HttpListenerRequest request, HttpListenerResponse response, Exception e)
        {
            string responseText = $"Error:\n" + e.Message;
            ErrorDTO errorDTO = new ErrorDTO(responseText);
            return JsonSerializer.Serialize(errorDTO);
        }

        public static async Task RunHttpListener(UserController userController)
        {
            var listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:8080/");

            listener.Start();
            Console.WriteLine("Listening on http://localhost:8080/  (Press Ctrl+C to exit)");

            try
            {
                while (true)
                {
                    var ctx = await listener.GetContextAsync();
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            HttpListenerRequest request = ctx.Request;
                            HttpListenerResponse response = ctx.Response;

                            // Remove trailing "/" if present
                            string url = request.RawUrl;
                            if(url.EndsWith("/"))
                            {
                                url = url.Substring(0, url.Length - 1);
                            }

                            string? authHeader = request.Headers["Authorization"];
                            if (authHeader != null && authHeader.StartsWith("Bearer "))
                            {
                                authHeader = authHeader.Substring(7);
                            }

                            // Read Request Body
                            StreamReader bodyStream = new StreamReader(request.InputStream);
                            string requestBodyText = bodyStream.ReadToEnd();

                            bool requestHandled = false;
                            string responseString = "";
                            // Regex: https://stackoverflow.com/questions/42707983/can-i-use-regex-expression-in-c-sharp-with-switch-case
                            try
                            {
                                switch (request.HttpMethod)
                                {
                                    case "GET":
                                        switch (true)
                                        {
                                            case bool _ when new Regex(@"^/api/users/[0-9]*/profile").IsMatch(url):
                                                requestHandled = true;
                                                string[] urlParts = url.Split("/");
                                                responseString = userController.GetProfile(authHeader, urlParts[3]);
                                                break;
                                        }
                                        break;
                                    case "POST":
                                        switch (true)
                                        {
                                            case bool _ when new Regex(@"^/api/users/login$").IsMatch(url):
                                                requestHandled = true;
                                                responseString = userController.Login(requestBodyText);
                                                break;
                                            case bool _ when new Regex(@"^/api/users/register").IsMatch(url):
                                                requestHandled = true;
                                                responseString = userController.Register(requestBodyText);
                                                break;
                                        }
                                        break;
                                    case "PUT":
                                        switch (true)
                                        {
                                            case bool _ when new Regex(@"^/api/users/[0-9]*/profile").IsMatch(url):
                                                requestHandled = true;
                                                string[] urlParts = url.Split("/");
                                                userController.UpdateProfile(authHeader, urlParts[3], requestBodyText);
                                                break;
                                        }
                                        break;
                                    case "DELETE":
                                        break;
                                }
                            }
                            catch (Exception e) when (e is InvalidRequestBodyException || e is UserAlreadyExistsException)
                            {
                                response.StatusCode = 400;
                                responseString = ErrorResponse(request, response, e);
                            }
                            catch (Exception e) when (e is InvalidCredentialException)
                            {
                                response.StatusCode = 401;
                                responseString = ErrorResponse(request, response, e);
                            }
                            catch (Exception e)
                            {
                                response.StatusCode = 500;
                                responseString = ErrorResponse(request, response, e);
                                Console.WriteLine("Unexpected Exception: " + e.Message);
                            }
                            if (!requestHandled)
                            {
                                response.StatusCode = 404;
                                responseString = NotFoundResponse(request, response);
                            }
                            response = await HttpUtility.WriteJsonToResponse(response, responseString);
                            response.Close();
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine(e);
                        }
                    });
                }
            }
            finally
            {
                listener.Stop();
                listener.Close();
            }
        }
    }
}
