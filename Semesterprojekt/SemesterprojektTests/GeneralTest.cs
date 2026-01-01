using Semesterprojekt.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterprojektTests
{
    [TestClass]
    public sealed class GeneralTest
    {
        [TestMethod]
        public void PasswordHashTest()
        {
            string password = "passwordInput1234!";
            string salt = "";
            string hash = PasswordHasher.HashPassword(password, out salt);

            Assert.AreEqual(hash, PasswordHasher.HashPassword(password, salt));
        }
    }
}
