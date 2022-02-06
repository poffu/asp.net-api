using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebAppAPI.Common
{
    public static class Common
    {
        /// <summary>
		/// Generate jwt
		/// </summary>
		/// <param name="_config">IConfiguration</param>
		/// <returns></returns>
        public static string GenerateJSONWebToken(IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
		/// Compare password input with Password DB
		/// </summary>
		/// <param name="password"></param>
		/// <param name="hashStr"></param>
		/// <returns></returns>
		public static bool SignIn(string password, string hashStr)
        {
            try
            {
                return PBKDF2.Verify(password, hashStr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Hash password 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string SignUp(string password)
        {
            return PBKDF2.Hash(password).ToString();
        }
    }
}