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
            GenreRepository genreRepository = new GenreRepository(databaseConnector);

            GenreService genreService = new GenreService(genreRepository);
            UserService userService = new UserService(userRepository, genreService);

            UserController userController = new UserController(userService, genreService);

            await MRPHttpListener.RunHttpListener(userController);
        }
    }
}
