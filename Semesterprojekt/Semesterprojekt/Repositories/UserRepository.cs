using Semesterprojekt.Controllers;
using Semesterprojekt.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Repositories
{
    public class UserRepository
    {
        static string filepath = "../../../data.csv";

        public static bool UserExists(string username)
        {
            IEnumerable<string> users = File.ReadAllLines(filepath);
            foreach (string user in users)
            {
                string[] infos = user.Split(";");
                if (infos[1] == username)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SaveUser(User user)
        {
            string saltString;
            string hashedPassword = PasswordHasher.HashPassword(user.Password, out saltString);
            string userEntry = user.UserId + ";" + user.Username + ";" + hashedPassword + ";" + saltString;

            File.AppendAllText(filepath, userEntry + Environment.NewLine);
        }

        public static int GetNewId()
        {
            IEnumerable<string> users = File.ReadAllLines(filepath);
            if(users.Count() > 0)
            {
                return Int32.Parse(users.Last().Split(";")[0]) + 1;
            }
            return 0;
        }
    }
}
