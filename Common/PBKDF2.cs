using System;
using System.Linq;
using System.Security.Cryptography;

namespace WebAppAPI.Common
{
    /// <summary>
    /// Class for PBKDF2
    /// </summary>
    public static class PBKDF2
    {
        private const int DerivedKeyLength = 160 / 8;
        /// <summary>
        /// Return hash by format PBKDF2Hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltSize"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static PBKDF2Hash Hash(string password, int saltSize = 16, int iterations = 10000)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize: saltSize, iterations: iterations))
            {
                var dk = deriveBytes.GetBytes(DerivedKeyLength);
                return new PBKDF2Hash(deriveBytes.IterationCount, deriveBytes.Salt, dk);
            }
        }

        /// <summary>
        /// Convert hashStr to format PBKDF2Hash then Verify password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashStr"></param>
        /// <returns></returns>
        public static bool Verify(string password, string hashStr)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (hashStr == null) throw new ArgumentNullException(nameof(hashStr));
            if (!PBKDF2Hash.TryParse(hashStr, out var hash)) throw new FormatException(nameof(hashStr));
            return Verify(password, hash);
        }

        /// <summary>
        /// Convert input password and compare hash (by format PBKDF2Hash)
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        private static bool Verify(string password, PBKDF2Hash hash)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (hash == null) throw new ArgumentNullException(nameof(hash));

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt: hash.Salt, iterations: hash.IterationCount))
            {
                var dk = deriveBytes.GetBytes(DerivedKeyLength);
                return hash.DerivedKey.SequenceEqual(dk);
            }
        }
    }
}