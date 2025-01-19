using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Helpers
{
    public class HashManager
    {
        public static string HashPassword(string input)
        {
            string salt = DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt(13);
            return DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(input, salt);
        }
        public static bool VerifyPassword(string input, string hashedPassword) => DevOne.Security.Cryptography.BCrypt.BCryptHelper.CheckPassword(input, hashedPassword);
    }
}