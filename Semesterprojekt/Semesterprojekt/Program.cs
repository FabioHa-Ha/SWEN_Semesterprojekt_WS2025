using Semesterprojekt.BusinessLayer;
using Semesterprojekt.Controllers;
using Semesterprojekt.General;
using Semesterprojekt.PersistenceLayer;
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
            MediaEntryRepository mediaEntryRepository = new MediaEntryRepository(databaseConnector);
            RatingRepository ratingRepository = new RatingRepository(databaseConnector);

            GenreService genreService = new GenreService(genreRepository);
            UserService userService = new UserService(userRepository, genreService);
            RatingService ratingService = new RatingService(ratingRepository, userService);
            MediaEntryService mediaEntryService = new MediaEntryService(mediaEntryRepository, genreService, ratingService);

            UserController userController = new UserController(userService, genreService);
            MediaEntryController mediaEntryController = new MediaEntryController(mediaEntryService, genreService,
                userService);
            RatingController ratingController = new RatingController(ratingService, userService, mediaEntryService);

            await MRPHttpListener.RunHttpListener(userController, mediaEntryController, ratingController);
        }
    }
}
