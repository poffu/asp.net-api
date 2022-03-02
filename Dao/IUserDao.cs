using System.Collections.Generic;
using WebAppAPI.Dto;

namespace WebAppAPI.Dao
{
    public interface IUserDao
    {
        public string Login(string email, string password);
        public List<UserDto> GetAllUser(string name);
        public bool IsExistEmail(string email, int userId = 0);
        public bool CreateUser(UserDto user);
        public UserDto GetUser(int userId);
        public bool UpdateUser(UserDto user);
        public bool DeleteUser(int userId);
    }
}