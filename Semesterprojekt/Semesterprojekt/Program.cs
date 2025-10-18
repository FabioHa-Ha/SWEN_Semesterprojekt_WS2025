using Semesterprojekt.Controllers;

namespace Semesterprojekt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await MRPHttpListener.RunHttpListener();
        }
    }
}
