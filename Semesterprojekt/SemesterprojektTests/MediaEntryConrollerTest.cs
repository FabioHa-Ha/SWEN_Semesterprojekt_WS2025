using Semesterprojekt.Controllers;
using Semesterprojekt.DTOs;
using Semesterprojekt.Repositories;
using Semesterprojekt.Services;
using System.Security.Authentication;
using System.Text.Json;

namespace SemesterprojektTests
{
    [TestClass]
    public sealed class MediaEntryControllerTest
    {
        private MediaEntryController mediaEntryController;
        private MediaEntryRepository mediaEntryRepository;
        private UserController userController;
        private UserRepository userRepository;

        [TestInitialize]
        public void Setup()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();

            userRepository = new UserRepository(databaseConnector);
            GenreRepository genreRepository = new GenreRepository(databaseConnector);
            RatingRepository ratingRepository = new RatingRepository(databaseConnector);
            mediaEntryRepository = new MediaEntryRepository(databaseConnector);

            GenreService genreService = new GenreService(genreRepository);
            UserService userService = new UserService(userRepository, genreService);
            RatingService ratingService = new RatingService(ratingRepository, userService);
            MediaEntryService mediaEntryService = new MediaEntryService(mediaEntryRepository, genreService, ratingService);

            userController = new UserController(userService, genreService);
            mediaEntryController = new MediaEntryController(mediaEntryService, genreService, userService);
        }

        [TestMethod]
        public void CreateAndGetMediaEntryTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserGetMediaEntry";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            // SETUP DONE

            MediaEntryUpdateDTO mediaEntryUpdateDTO = new MediaEntryUpdateDTO("testTitle", "testDescription",
                "Movie", 2000, 18, ["a", "b"]);
            requestString = JsonSerializer.Serialize(mediaEntryUpdateDTO);
            int newId = mediaEntryController.CreateMedia(jwtDTO.token, requestString);

            responseString = mediaEntryController.GetMediaEntry(jwtDTO.token, newId.ToString());
            MediaEntryDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(responseString);

            Assert.IsNotNull(mediaEntryDTO);
            Assert.AreEqual(mediaEntryUpdateDTO.title, mediaEntryDTO.title);
            Assert.AreEqual(mediaEntryUpdateDTO.description, mediaEntryDTO.description);
            Assert.AreEqual(mediaEntryUpdateDTO.mediaType, mediaEntryDTO.mediaType);
            Assert.AreEqual(mediaEntryUpdateDTO.releaseYear, mediaEntryDTO.releaseYear);
            Assert.AreEqual(mediaEntryUpdateDTO.ageRestriction, mediaEntryDTO.ageRestriction);

            // TEARDOWN

            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void DeleteMediaEntryTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserDeleteMediaEntry";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            MediaEntryUpdateDTO mediaEntryUpdateDTO = new MediaEntryUpdateDTO("testTitle", "testDescription",
                "Movie", 2000, 18, ["a", "b"]);
            requestString = JsonSerializer.Serialize(mediaEntryUpdateDTO);
            int newId = mediaEntryController.CreateMedia(jwtDTO.token, requestString);

            // SETUP DONE

            mediaEntryController.DeleteMedia(jwtDTO.token, newId.ToString());

            // TEARDOWN

            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void DeleteMediaEntryFailTest()
        {
            string username1 = "testFailUserDeleteMediaEntry1";
            string username2 = "testFailUserDeleteMediaEntry2";
            UserInfoDTO user = new UserInfoDTO();
            user.username = username1;
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            MediaEntryUpdateDTO mediaEntryUpdateDTO = new MediaEntryUpdateDTO("testTitle", "testDescription",
                "Movie", 2000, 18, ["a", "b"]);
            requestString = JsonSerializer.Serialize(mediaEntryUpdateDTO);
            int newId = mediaEntryController.CreateMedia(jwtDTO.token, requestString);


            user.username = username2;

            requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            responseString = userController.Login(requestString);
            jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            userId = userRepository.GetUserByUsername(user.username).UserId;

            // SETUP DONE

            Assert.ThrowsException<UnauthorizedAccessException>(() => 
                mediaEntryController.DeleteMedia(jwtDTO.token, newId.ToString()));

            // TEARDOWN

            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(username1);
            userRepository.DeleteUser(username2);
        }

        [TestMethod]
        public void UpdateMediaEntryTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserUpdateMediaEntry";
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            MediaEntryUpdateDTO mediaEntryUpdateDTO = new MediaEntryUpdateDTO("testTitle", "testDescription",
                "Movie", 2000, 18, ["a", "b"]);
            requestString = JsonSerializer.Serialize(mediaEntryUpdateDTO);
            int newId = mediaEntryController.CreateMedia(jwtDTO.token, requestString);

            // SETUP DONE

            mediaEntryUpdateDTO = new MediaEntryUpdateDTO("testTitleUpdated", "testDescriptionUpdated",
                "Movie", 2026, 16, ["c", "d"]);
            requestString = JsonSerializer.Serialize(mediaEntryUpdateDTO);
            mediaEntryController.UpdateMedia(jwtDTO.token, newId.ToString(), requestString);

            responseString = mediaEntryController.GetMediaEntry(jwtDTO.token, newId.ToString());
            MediaEntryDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(responseString);

            Assert.IsNotNull(mediaEntryDTO);
            Assert.AreEqual(mediaEntryUpdateDTO.title, mediaEntryDTO.title);
            Assert.AreEqual(mediaEntryUpdateDTO.description, mediaEntryDTO.description);
            Assert.AreEqual(mediaEntryUpdateDTO.mediaType, mediaEntryDTO.mediaType);
            Assert.AreEqual(mediaEntryUpdateDTO.releaseYear, mediaEntryDTO.releaseYear);
            Assert.AreEqual(mediaEntryUpdateDTO.ageRestriction, mediaEntryDTO.ageRestriction);

            // TEARDOWN

            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(user.username);
        }
    }
}
