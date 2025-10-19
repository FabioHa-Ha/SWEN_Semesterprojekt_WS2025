using Semesterprojekt.General;
using Semesterprojekt.Repositories;

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
