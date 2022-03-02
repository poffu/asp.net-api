using System.Collections.Generic;
using System.Linq;
using WebAppAPI.Dao.Model;
using WebAppAPI.Dto;
using Microsoft.Extensions.Configuration;

namespace WebAppAPI.Dao
{
    public class UserDao : IUserDao
    {
        private readonly UserContext _userContext;
        private IConfiguration _config;

        public UserDao(UserContext userContext, IConfiguration config)
        {
            this._userContext = userContext;
            this._config = config;
        }

        public string Login(string email, string password)
        {
            string token = string.Empty;
            try
            {
                var user = _userContext.WebUserModels.Where(m => (m.email == email) && (m.rule == 0)).FirstOrDefault();
                if (user != null)
                {
                    if (Common.Common.SignIn(password, user.password))
                    {
                        token = Common.Common.GenerateJSONWebToken(_config);
                    }
                }
            }
            catch
            {
                throw;
            }
            return token;
        }

        public List<UserDto> GetAllUser(string name)
        {
            List<UserDto> userDtoList = new List<UserDto>();
            try
            {
                var userList = _userContext.WebUserModels.Where(m => (m.name.Contains(name) && (m.rule == 1))).OrderBy(m => m.name).ThenBy(m => m.email).ToList();
                if (userList.Count > 0)
                {
                    foreach (WebUserModel user in userList)
                    {
                        userDtoList.Add(ConvertUser(user));
                    }
                }
            }
            catch
            {
                throw;
            }
            return userDtoList;
        }

        public bool IsExistEmail(string email, int userId)
        {
            bool check = false;
            try
            {
                var user = _userContext.WebUserModels.Where(m => (m.email == email) && (m.user_id != userId)).FirstOrDefault();
                if (user != null)
                {
                    check = true;
                }
            }
            catch
            {
                throw;
            }
            return check;
        }

        public bool CreateUser(UserDto user)
        {
            bool check = false;
            try
            {
                var userDB = _userContext.WebUserModels.Add(new WebUserModel
                {
                    name = user.Name,
                    email = user.Email,
                    tel = user.Tel,
                    password = Common.Common.SignUp(user.Password),
                    rule = 1
                });
                _userContext.SaveChanges();
                check = true;
            }
            catch
            {
                throw;
            }
            return check;
        }

        public UserDto GetUser(int userId)
        {
            UserDto userDto = new UserDto();
            try
            {
                var user = _userContext.WebUserModels.Where(m => m.user_id == userId).FirstOrDefault();
                if (user != null)
                {
                    userDto = ConvertUser(user);
                }
            }
            catch
            {
                throw;
            }
            return userDto;
        }

        public bool UpdateUser(UserDto user)
        {
            bool check = false;
            try
            {
                var userDB = _userContext.WebUserModels.Update(new WebUserModel
                {
                    user_id = user.UserId,
                    name = user.Name,
                    email = user.Email,
                    tel = user.Tel,
                    password = Common.Common.SignUp(user.Password),
                    rule = 1
                });
                _userContext.SaveChanges();
                check = true;
            }
            catch
            {
                throw;
            }
            return check;
        }

        public bool DeleteUser(int userId)
        {
            bool check = false;
            try
            {
                var user = _userContext.WebUserModels.Where(m => m.user_id == userId).FirstOrDefault();
                if (user != null)
                {
                    _userContext.WebUserModels.Remove(user);
                    _userContext.SaveChanges();
                }
                check = true;
            }
            catch
            {
                throw;
            }
            return check;
        }

        private UserDto ConvertUser(WebUserModel user)
        {
            UserDto userDto = new UserDto
            {
                UserId = user.user_id,
                Name = user.name,
                Email = user.email,
                Tel = user.tel,
                Password = user.password
            };
            return userDto;
        }
    }
}