using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;

namespace Vimba.AviTrade.Web.Helpers
{
    /// <summary>
    /// This class is copied from some MVC project sample on Microsoft site
    /// </summary>
    public static class PasswordHelper
    {
        private static SHA512 ShaProvider = SHA512.Create();
        private static Encoding Encoder = Encoding.UTF8;

        private static string EchaleSal(string password)
        {
            var salt = ConfigurationManager.AppSettings["PasswordSalt"] ?? "{0}";
            return string.Format(salt, password);
        }

        public static string GetHashedPassword(string password)
        {
            var bytesFromString = Encoder.GetBytes(EchaleSal(password));
            byte[] bytesHashed = ShaProvider.ComputeHash(bytesFromString);

            var stringHashed = bytesHashed.Aggregate(string.Empty, (x, y) => x + (y).ToString("X").PadLeft(2, '0'));

            return stringHashed;
        }
    }
}