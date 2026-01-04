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

        public static async Task RunHttpListener(UserController userController, 
            MediaEntryController mediaEntryController, RatingController ratingController)
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
                            string[] urlParts;
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
                                                urlParts = url.Split("/");
                                                responseString = userController.GetProfile(authHeader, urlParts[3]);
                                                break;
                                            case bool _ when new Regex(@"^/api/media/[0-9]*").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                responseString = mediaEntryController.GetMediaEntry(urlParts[3]);
                                                break;
                                            case bool _ when new Regex(@"^/api/media").IsMatch(url):
                                                requestHandled = true;
                                                responseString = mediaEntryController.GetAllMediaEntries();
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
                                            case bool _ when new Regex(@"^/api/media/[0-9]*/rate").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                ratingController.CreateRating(authHeader, urlParts[3], requestBodyText);
                                                break;
                                            case bool _ when new Regex(@"^/api/media").IsMatch(url):
                                                requestHandled = true;
                                                mediaEntryController.CreateMedia(authHeader, requestBodyText);
                                                break;
                                            case bool _ when new Regex(@"^/api/ratings/[0-9]*/like").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                ratingController.LikeRating(authHeader, urlParts[3]);
                                                break;
                                            case bool _ when new Regex(@"^/api/ratings/[0-9]*/confirm").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                ratingController.ConfirmRating(authHeader, urlParts[3]);
                                                break;
                                        }
                                        break;
                                    case "PUT":
                                        switch (true)
                                        {
                                            case bool _ when new Regex(@"^/api/users/[0-9]*/profile").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                userController.UpdateProfile(authHeader, urlParts[3], requestBodyText);
                                                break;
                                            case bool _ when new Regex(@"^/api/media/[0-9]*").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                mediaEntryController.UpdateMedia(authHeader, urlParts[3], requestBodyText);
                                                break;
                                            case bool _ when new Regex(@"^/api/ratings/[0-9]*").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                ratingController.UpdateRating(authHeader, urlParts[3], requestBodyText);
                                                break;
                                        }
                                        break;
                                    case "DELETE":
                                        switch (true)
                                        {
                                            case bool _ when new Regex(@"^/api/media/[0-9]*").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                mediaEntryController.DeleteMedia(authHeader, urlParts[3]);
                                                break;
                                            case bool _ when new Regex(@"^/api/ratings/[0-9]*").IsMatch(url):
                                                requestHandled = true;
                                                urlParts = url.Split("/");
                                                ratingController.DeleteRating(authHeader, urlParts[3]);
                                                break;
                                        }
                                        break;
                                }
                            }
                            catch (Exception e) when (e is InvalidRequestBodyException || 
                                e is UserAlreadyExistsException || e is UnkownMediaEntryException || 
                                e is InvalidStarRatingExcption || e is InvalidAccessException ||
                                e is UnkownRatingException)
                            {
                                response.StatusCode = 400;
                                responseString = ErrorResponse(request, response, e);
                            }
                            catch (Exception e) when (e is InvalidCredentialException)
                            {
                                response.StatusCode = 401;
                                responseString = ErrorResponse(request, response, e);
                            }
                            catch (Exception e) when (e is UnauthorizedAccessException)
                            {
                                response.StatusCode = 403;
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
