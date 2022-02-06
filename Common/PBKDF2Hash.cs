using System;
using System.Text;

namespace WebAppAPI.Common
{
    /// <summary>
    /// Class for PBKDF2Hash
    /// </summary>
    public class PBKDF2Hash
    {
        private const string HashId = "pbkdf2";

        /// <summary>
        /// constructor for PBKDF2Hash
        /// </summary>
        /// <param name="iterationCount"></param>
        /// <param name="salt"></param>
        /// <param name="derivedKey"></param>
        public PBKDF2Hash(int iterationCount, byte[] salt, byte[] derivedKey)
        {
            IterationCount = iterationCount;
            Salt = salt;
            DerivedKey = derivedKey;
        }

        public int IterationCount { get; }
        public byte[] Salt { get; }
        public byte[] DerivedKey { get; }

        /// <summary>
        /// override ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("${0}${1}${2}${3}", HashId, IterationCount, AdaptedBase64Encode(Salt), AdaptedBase64Encode(DerivedKey));
        }

        /// <summary>
        /// TryPase string to PBKDF2Hash
        /// </summary>
        /// <param name="hashStr"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string hashStr, out PBKDF2Hash result)
        {
            result = null;
            if (hashStr == null) return false;
            if (!hashStr.StartsWith("$")) return false;

            var elems = hashStr.Split(new[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
            if (elems.Length != 4) return false;

            if (elems[0] != HashId) return false;
            if (!int.TryParse(elems[1], out var iterationCount)) return false;
            var salt = AdaptedBase64Decode(elems[2]);
            var dk = AdaptedBase64Decode(elems[3]);

            result = new PBKDF2Hash(iterationCount, salt, dk);
            return true;
        }

        #region Helper
        /// <summary>
        /// adapted base64 encoding
        /// http://nullege.com/codes/search/passlib.utils.ab64_encode
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        private static string AdaptedBase64Encode(byte[] bin)
        {
            return Convert.ToBase64String(bin).TrimEnd('=').Replace('+', '.');
        }

        /// <summary>
        /// http://nullege.com/codes/search/passlib.utils.ab64_decode
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static byte[] AdaptedBase64Decode(string value)
        {
            var paddingLen = (4 - value.Length % 4) & 0x3;
            var bldr = new StringBuilder(value);
            bldr.Replace('.', '+');
            bldr.Append('=', paddingLen);
            return Convert.FromBase64String(bldr.ToString());
        }
        #endregion Helper
    }
}