using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Controllers
{
    internal class HttpUtility
    {
        public static async Task<HttpListenerResponse> WriteJsonToResponse(HttpListenerResponse response, string jsonText)
        {
            byte[] bytes;
            bytes = Encoding.UTF8.GetBytes(jsonText);

            response.ContentType = "application/json; charset=utf-8";
            response.ContentLength64 = bytes.Length;
            await response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
            return response;
        }

        public static async Task<HttpListenerResponse> WriteTextToResponse(HttpListenerResponse response, string jsonText)
        {
            byte[] bytes;
            bytes = Encoding.UTF8.GetBytes(jsonText);

            response.ContentType = "text/plain; charset=utf-8";
            response.ContentLength64 = bytes.Length;
            await response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
            return response;
        }
    }
}
