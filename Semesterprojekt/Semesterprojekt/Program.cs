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

            UserService userService = new UserService(userRepository);

            UserController userController = new UserController(userService);

            await MRPHttpListener.RunHttpListener(userController);
        }
    }
}
