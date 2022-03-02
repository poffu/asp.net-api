using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAppAPI.Dao;
using WebAppAPI.Dto;

namespace WebAppAPI.Controller
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
        private readonly IUserDao _userDao;

        public MainController(UserContext userContext, IConfiguration config) => _userDao = new UserDao(userContext, config);

        [AllowAnonymous]
        [HttpGet]
        public string Root()
        {
            return "Hello world";
        }

        [AllowAnonymous]
        [Route("/login")]
        [HttpPost]
        public IActionResult Login(string email = "", string password = "")
        {
            try
            {
                var token = _userDao.Login(email, password);
                if (string.IsNullOrEmpty(token))
                {
                    return Problem(statusCode: 404, detail: "Email or Password is not correct.");
                }
                return Ok(new { token = token });
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [Route("/list-user")]
        [HttpGet]
        public IActionResult ListUser(string name = "")
        {
            try
            {
                var user = _userDao.GetAllUser(name);
                return Ok(user);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [Route("/add-user")]
        [HttpPost]
        public IActionResult AddUser(UserDto userRequest)
        {
            try
            {
                bool isExistEmail = _userDao.IsExistEmail(userRequest.Email);
                if (isExistEmail)
                {
                    return Problem(statusCode: 400, detail: "Email is exist.");
                }
                bool isCreate = _userDao.CreateUser(userRequest);
                return Ok(isCreate);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [Route("/get-user")]
        [HttpGet]
        public IActionResult GetUser(int userId = 0)
        {
            try
            {
                var user = _userDao.GetUser(userId);
                if (user.UserId == 0)
                {
                    return Problem(statusCode: 404, detail: "Data doesn't exist.");
                }
                return Ok(user);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [Route("/edit-user")]
        [HttpPut]
        public IActionResult EditUser(UserDto userRequest)
        {
            try
            {
                bool isExistEmail = _userDao.IsExistEmail(userRequest.Email, userRequest.UserId);
                if (isExistEmail)
                {
                    return Problem(statusCode: 400, detail: "Email is exist.");
                }
                bool isUpdate = _userDao.UpdateUser(userRequest);
                return Ok(isUpdate);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [Route("/delete-user")]
        [HttpDelete]
        public IActionResult DeleteUser(int userId = 0)
        {
            try
            {
                bool isDelete = _userDao.DeleteUser(userId);
                return Ok(isDelete);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}