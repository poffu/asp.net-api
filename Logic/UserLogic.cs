using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using WebAppAPI.Dao;
using WebAppAPI.Dto;

namespace WebAppAPI.Logic
{
    public class UserLogic : IUserLogic
    {
        private IUserDao _userDao;
        private IConfiguration _config;

        public UserLogic(UserContext userContext, IConfiguration config)
        {
            _userDao = new UserDao(userContext);
            _config = config;
        }

        public string Login(string email, string password)
        {
            string token = string.Empty;
            if (_userDao.IsLogin(email, password))
            {
                token = Common.Common.GenerateJSONWebToken(_config);
            }
            return token;
        }

        public List<UserDto> GetAllUser(string name)
        {
            return _userDao.GetAllUser(name);
        }

        public bool IsExistEmail(string email, int userId = 0)
        {
            return _userDao.IsExistEmail(email, userId);
        }

        public bool CreateUser(UserDto user)
        {
            return _userDao.CreateUser(user);
        }

        public UserDto GetUser(int userId)
        {
            return _userDao.GetUser(userId);
        }

        public bool UpdateUser(UserDto user)
        {
            return _userDao.UpdateUser(user);
        }

        public bool DeleteUser(int userId)
        {
            return _userDao.DeleteUser(userId);
        }
    }
}