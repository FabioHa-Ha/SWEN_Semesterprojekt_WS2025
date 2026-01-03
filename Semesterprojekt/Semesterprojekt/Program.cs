using Semesterprojekt.Controllers;
using Semesterprojekt.General;
using Semesterprojekt.Repositories;
using Semesterprojekt.Services;

namespace Semesterprojekt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();

            UserRepository userRepository = new UserRepository(databaseConnector);

            AuthService authService = new AuthService(userRepository);

            AuthController authController = new AuthController(authService);

            await MRPHttpListener.RunHttpListener(authController);
        }
    }
}
