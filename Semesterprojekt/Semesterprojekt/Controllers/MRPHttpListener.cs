using Semesterprojekt.Exceptions;
using Semesterprojekt.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Controllers
{
    public class MRPHttpListener
    {
        public static async Task<HttpListenerResponse> NotFoundResponse(HttpListenerRequest request, HttpListenerResponse response)
        {
            response.StatusCode = 404;
            string responseText = $"Error 404:\nThe requested page: {request.HttpMethod} {request.Url} is not available";
            return await HttpUtility.WriteTextToResponse(response, responseText);
        }

        public static async Task<HttpListenerResponse> ErrorResponse(HttpListenerRequest request, HttpListenerResponse response, Exception e, int statusCode)
        {
            response.StatusCode = statusCode;
            string responseText = $"Error:\n" + e.Message;
            return await HttpUtility.WriteTextToResponse(response, responseText);
        }

        public static async Task RunHttpListener()
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

                            // Read Request Body
                            StreamReader bodyStream = new StreamReader(request.InputStream);
                            string requestBodyText = bodyStream.ReadToEnd();

                            bool requestHandled = false;
                            try
                            {
                                switch (request.HttpMethod)
                                {
                                    case "GET":
                                        break;
                                    case "POST":
                                        switch (url)
                                        {
                                            case "/api/users/login":
                                                requestHandled = true;
                                                response = await AuthController.Login(requestBodyText, response);
                                                break;
                                            case "/api/users/register":
                                                requestHandled = true;
                                                response = await AuthController.Register(requestBodyText, response);
                                                break;
                                        }
                                        break;
                                    case "PUT":
                                        break;
                                    case "DELETE":
                                        break;
                                }
                            }
                            catch (Exception e) when (e is InvalidRequestBodyException || e is UserAlreadyExistsException)
                            {
                                response = await ErrorResponse(request, response, e, 400);
                            }
                            catch (Exception e)
                            {
                                response = await ErrorResponse(request, response, e, 500);
                                Console.WriteLine("Unexpected Exception: " + e.Message);
                            }
                            if (!requestHandled)
                            {
                                response = await NotFoundResponse(request, response);
                            }
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
