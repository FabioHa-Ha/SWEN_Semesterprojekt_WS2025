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
            string responseBody = "";
            byte[] bytes;
            responseBody = $"Error 404:\nThe requested page: {request.HttpMethod} {request.Url} is not available";
            bytes = Encoding.UTF8.GetBytes(responseBody);

            response.ContentType = "text/plain; charset=utf-8";
            response.ContentLength64 = bytes.Length;
            await response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
            return response;
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
                            string url = request.RawUrl;
                            if(url.EndsWith("/"))
                            {
                                url = url.Substring(0, url.Length - 1);
                            }
                            bool requestHandled = false;
                            switch (request.HttpMethod)
                            {
                                case "GET":
                                    break;
                                case "POST":
                                    switch (url)
                                    {
                                        case "/api/users/login":
                                            response = AuthController.Login(request, response);
                                            requestHandled = true;
                                            break;
                                        case "/api/users/register":
                                            response = AuthController.Register(request, response);
                                            requestHandled = true;
                                            break;
                                    }
                                    break;
                                case "PUT":
                                    break;
                                case "DELETE":
                                    break;
                            }
                            if (!requestHandled)
                            {
                                response = await NotFoundResponse(request, response);
                            }
                            response.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex);
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
