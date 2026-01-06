using Semesterprojekt.Controllers;
using Semesterprojekt.DTOs;
using Semesterprojekt.Exceptions;
using Semesterprojekt.Repositories;
using Semesterprojekt.Services;
using System.Text.Json;

namespace SemesterprojektTests
{
    [TestClass]
    public sealed class RatingControllerTest
    {

        private MediaEntryRepository mediaEntryRepository;
        private UserRepository userRepository;
        private RatingRepository ratingRepository;

        private MediaEntryController mediaEntryController;
        private UserController userController;
        private RatingController ratingController;

        [TestInitialize]
        public void Setup()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();

            userRepository = new UserRepository(databaseConnector);
            GenreRepository genreRepository = new GenreRepository(databaseConnector);
            ratingRepository = new RatingRepository(databaseConnector);
            mediaEntryRepository = new MediaEntryRepository(databaseConnector);

            GenreService genreService = new GenreService(genreRepository);
            UserService userService = new UserService(userRepository, genreService);
            RatingService ratingService = new RatingService(ratingRepository, userService);
            MediaEntryService mediaEntryService = new MediaEntryService(mediaEntryRepository, genreService, ratingService);

            userController = new UserController(userService, genreService);
            mediaEntryController = new MediaEntryController(mediaEntryService, genreService, userService);
            ratingController = new RatingController(ratingService, userService, mediaEntryService);
        }

        [TestMethod]
        public void RateMediaTest()
        {
            string username1 = "testUserRateMedia1";
            string username2 = "testUserRateMedia2";
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

            RatingDTO ratingDTO = new RatingDTO(5, "Nice!");
            requestString = JsonSerializer.Serialize(ratingDTO);
            int ratingId = ratingController.CreateRating(jwtDTO.token, newId.ToString(), requestString);
            ratingController.ConfirmRating(jwtDTO.token, ratingId.ToString());

            responseString = mediaEntryController.GetMediaEntry(jwtDTO.token, newId.ToString());
            MediaEntryDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(responseString);

            Assert.AreEqual(ratingDTO.stars, mediaEntryDTO.ratings[0].star_rating);
            Assert.AreEqual(ratingDTO.comment, mediaEntryDTO.ratings[0].comment);

            // TEARDOWN

            ratingRepository.DeleteRating(ratingId);
            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void RateMediaFailTest()
        {
            UserInfoDTO user = new UserInfoDTO();
            user.username = "testUserRateMediaFail";
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

            RatingDTO ratingDTO = new RatingDTO(5, "Nice!");
            requestString = JsonSerializer.Serialize(ratingDTO);

            Assert.ThrowsException<InvalidAccessException>(() =>
                ratingController.CreateRating(jwtDTO.token, newId.ToString(), requestString));

            // TEARDOWN

            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void UpdateRatingTest()
        {
            string username1 = "testUserUpdateRating1";
            string username2 = "testUserUpdateRating2";
            UserInfoDTO user = new UserInfoDTO();
            user.username = username1;
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO1 = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            MediaEntryUpdateDTO mediaEntryUpdateDTO = new MediaEntryUpdateDTO("testTitle", "testDescription",
                "Movie", 2000, 18, ["a", "b"]);
            requestString = JsonSerializer.Serialize(mediaEntryUpdateDTO);
            int newId = mediaEntryController.CreateMedia(jwtDTO1.token, requestString);


            user.username = username2;

            requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            responseString = userController.Login(requestString);
            JwtDTO jwtDTO2 = JsonSerializer.Deserialize<JwtDTO>(responseString);

            userId = userRepository.GetUserByUsername(user.username).UserId;


            RatingDTO ratingDTO = new RatingDTO(5, "Nice!");
            requestString = JsonSerializer.Serialize(ratingDTO);
            int ratingId = ratingController.CreateRating(jwtDTO2.token, newId.ToString(), requestString);
            ratingController.ConfirmRating(jwtDTO2.token, ratingId.ToString());

            // SETUP DONE

            ratingDTO = new RatingDTO(1, "Terrible!");
            ratingController.UpdateRating(jwtDTO2.token, ratingId.ToString(), JsonSerializer.Serialize(ratingDTO));

            responseString = mediaEntryController.GetMediaEntry(jwtDTO2.token, newId.ToString());
            MediaEntryDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(responseString);

            Assert.AreEqual(ratingDTO.stars, mediaEntryDTO.ratings[0].star_rating);
            Assert.AreEqual(ratingDTO.comment, mediaEntryDTO.ratings[0].comment);

            // TEARDOWN

            ratingRepository.DeleteRating(ratingId);
            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void UpdateRatingFailTest()
        {
            string username1 = "testUserUpdateRatingFail1";
            string username2 = "testUserUpdateRatingFail2";
            UserInfoDTO user = new UserInfoDTO();
            user.username = username1;
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO1 = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            MediaEntryUpdateDTO mediaEntryUpdateDTO = new MediaEntryUpdateDTO("testTitle", "testDescription",
                "Movie", 2000, 18, ["a", "b"]);
            requestString = JsonSerializer.Serialize(mediaEntryUpdateDTO);
            int newId = mediaEntryController.CreateMedia(jwtDTO1.token, requestString);


            user.username = username2;

            requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            responseString = userController.Login(requestString);
            JwtDTO jwtDTO2 = JsonSerializer.Deserialize<JwtDTO>(responseString);


            RatingDTO ratingDTO = new RatingDTO(5, "Nice!");
            requestString = JsonSerializer.Serialize(ratingDTO);
            int ratingId = ratingController.CreateRating(jwtDTO2.token, newId.ToString(), requestString);


            // SETUP DONE

            ratingDTO = new RatingDTO(1, "Terrible!");
            Assert.ThrowsException<InvalidAccessException>(() =>
                ratingController.UpdateRating(jwtDTO1.token, ratingId.ToString(), JsonSerializer.Serialize(ratingDTO)));

            // TEARDOWN

            ratingRepository.DeleteRating(ratingId);
            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(username1);
            userRepository.DeleteUser(username2);
        }

        [TestMethod]
        public void DeleteRatingTest()
        {
            string username1 = "testUserDeleteRating1";
            string username2 = "testUserDeleteRating2";
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


            RatingDTO ratingDTO = new RatingDTO(5, "Nice!");
            requestString = JsonSerializer.Serialize(ratingDTO);
            int ratingId = ratingController.CreateRating(jwtDTO.token, newId.ToString(), requestString);
            ratingController.ConfirmRating(jwtDTO.token, ratingId.ToString());

            // SETUP DONE

            ratingController.DeleteRating(jwtDTO.token, ratingId.ToString());

            responseString = mediaEntryController.GetMediaEntry(jwtDTO.token, newId.ToString());
            MediaEntryDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(responseString);

            Assert.AreEqual(0, mediaEntryDTO.ratings.Count());

            // TEARDOWN

            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(user.username);
        }

        [TestMethod]
        public void ConfirmRatingTest()
        {
            string username1 = "testUserConfirmRating1";
            string username2 = "testUserConfirmRating2";
            UserInfoDTO user = new UserInfoDTO();
            user.username = username1;
            user.password = "password";

            string requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            string responseString = userController.Login(requestString);
            JwtDTO jwtDTO1 = JsonSerializer.Deserialize<JwtDTO>(responseString);

            int userId = userRepository.GetUserByUsername(user.username).UserId;

            MediaEntryUpdateDTO mediaEntryUpdateDTO = new MediaEntryUpdateDTO("testTitle", "testDescription",
                "Movie", 2000, 18, ["a", "b"]);
            requestString = JsonSerializer.Serialize(mediaEntryUpdateDTO);
            int newId = mediaEntryController.CreateMedia(jwtDTO1.token, requestString);


            user.username = username2;

            requestString = JsonSerializer.Serialize(user);
            userController.Register(requestString);

            responseString = userController.Login(requestString);
            JwtDTO jwtDTO2 = JsonSerializer.Deserialize<JwtDTO>(responseString);

            userId = userRepository.GetUserByUsername(user.username).UserId;


            RatingDTO ratingDTO = new RatingDTO(5, "Nice!");
            requestString = JsonSerializer.Serialize(ratingDTO);
            int ratingId = ratingController.CreateRating(jwtDTO2.token, newId.ToString(), requestString);

            // SETUP DONE

            responseString = mediaEntryController.GetMediaEntry(jwtDTO1.token, newId.ToString());
            MediaEntryDTO mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(responseString);

            Assert.AreEqual(0, mediaEntryDTO.ratings.Count());

            ratingController.ConfirmRating(jwtDTO2.token, ratingId.ToString());

            responseString = mediaEntryController.GetMediaEntry(jwtDTO1.token, newId.ToString());
            mediaEntryDTO = JsonSerializer.Deserialize<MediaEntryDTO>(responseString);

            Assert.AreEqual(1, mediaEntryDTO.ratings.Count());

            // TEARDOWN

            mediaEntryRepository.DeleteMediaEntry(newId);
            userRepository.DeleteUser(user.username);
        }
    }
}
