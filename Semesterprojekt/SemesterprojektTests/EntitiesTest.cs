using Semesterprojekt.Entities;
using Semesterprojekt.Exceptions;

namespace SemesterprojektTests
{
    [TestClass]
    public sealed class EntitiesTest
    {
        [TestMethod]
        public void CreateMovieTest()
        {
            string title = "Star Wars";
            string description = "testDescription";
            int releaseYear = 1977;
            string genre = "Sci Fi";
            int ageRestriction = 12;
            int creatorId = 1;
            Movie movie = new Movie(1, title, description, releaseYear, ageRestriction, creatorId);
            Assert.AreEqual(title, movie.Title);
            Assert.AreEqual(description, movie.Description);
            Assert.AreEqual(releaseYear, movie.ReleaseYear);
            Assert.AreEqual(ageRestriction, movie.AgeRestriction);
            Assert.AreEqual(creatorId, movie.Creator);
        }

        [TestMethod]
        public void CreateGameTest()
        {
            string title = "Tetris";
            string description = "testDescription";
            int releaseYear = 1984;
            string genre = "Arcade";
            int ageRestriction = 0;
            int creatorId = 1;
            Game game = new Game(1, title, description, releaseYear, ageRestriction, creatorId);
            Assert.AreEqual(title, game.Title);
            Assert.AreEqual(description, game.Description);
            Assert.AreEqual(releaseYear, game.ReleaseYear);
            Assert.AreEqual(ageRestriction, game.AgeRestriction);
            Assert.AreEqual(creatorId, game.Creator);
        }

        [TestMethod]
        public void CreateSeriesTest()
        {
            string title = "Gravity Falls";
            string description = "testDescription";
            int releaseYear = 2012;
            string genre = "Cartoon";
            int ageRestriction = 7;
            int creatorId = 1;
            Series series = new Series(1, title, description, releaseYear, ageRestriction, creatorId);
            Assert.AreEqual(title, series.Title);
            Assert.AreEqual(description, series.Description);
            Assert.AreEqual(releaseYear, series.ReleaseYear);
            Assert.AreEqual(ageRestriction, series.AgeRestriction);
            Assert.AreEqual(creatorId, series.Creator);
        }

        [TestMethod]
        public void CreateRatingSuccessTest()
        {
            int userId = 1;
            int mediaEntryId = 1;
            int starRating = 3;
            string comment = "Nice!";
            Rating rating = new Rating(1, userId, mediaEntryId, starRating, comment);
            Assert.AreEqual(userId, rating.Creator);
            Assert.AreEqual(mediaEntryId, rating.OfMediaEntry);
            Assert.AreEqual(starRating, rating.StarRating);
            Assert.AreEqual(comment, rating.Comment);
        }

        [TestMethod]
        public void CreateRatingFailTest()
        {
            int userId = 1;
            int mediaEntryId = 1;
            int starRating = 6;
            string comment = "Nice!";
            Assert.ThrowsException<InvalidStarRatingExcption>(() => new Rating(1, userId, mediaEntryId, starRating, comment));
        }

        [TestMethod]
        public void CreateUserTest()
        {
            string username = "User1";
            string password = "asdf1234";
            User user = new User(1, username, password);
            Assert.AreEqual(username, user.Username);
            Assert.AreEqual(password, user.Password);
        }
    }
}
