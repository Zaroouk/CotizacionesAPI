using Cotizaciones.Data;
using Cotizaciones.Models;
using Cotizaciones.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Cotizaciones.Controllers
{
    [ApiController]
    [Route("Api/User")]
    [Authorize(Roles = "Admin,User")]
    public class UserController : ControllerBase
    {
        IUserRepository _userRepository;
        IMapper _mapper;
        public UserController(IConfiguration config, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
            }));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _userRepository.GetUsers();
            if (users != null)
            {
                return users;
            }
            throw new Exception("No users Found");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            User user = _userRepository.GetSingleUser(userId);
            if (user != null)
            {
                return user;
            }
            throw new Exception("This user does not exist");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public IActionResult CreateUser(UserToAddDto newUser)
        {
            User userDb = _mapper.Map<User>(newUser);
            _userRepository.AddEntity<User>(userDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to add User");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            int.TryParse(User.FindFirst("userId")?.Value, out int userId);
            User userDb = _userRepository.GetSingleUser(userId);

            if (userDb != null)
            {
                userDb.Active = user.Active;
                userDb.FullName = user.FullName;
                userDb.Email = user.Email;
                userDb.Role = user.Role;
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Failed to Update User");
            }
            throw new Exception("Failed to Get User");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            // int.TryParse(User.FindFirst("userId")?.Value, out int userId);
            User? user = _userRepository.GetSingleUser(userId);
            if (user != null)
            {
                _userRepository.RemoveEntity<User>(user);
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Failed to Delete User");
            }
            throw new Exception("Failed to get User");
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetCurrentUserInfo")]
        public IActionResult GetCurrentUserInfo()
        {
            int.TryParse(User.FindFirst("userId")?.Value, out int userId);
            User user = _userRepository.GetSingleUser(userId);
            return Ok(user);
        }
    }
}