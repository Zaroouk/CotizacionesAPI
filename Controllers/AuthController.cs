using System.Security.Cryptography;
using AutoMapper;
using Cotizaciones.Data;
using Cotizaciones.Dtos;
using Cotizaciones.Helpers;
using Cotizaciones.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cotizaciones.Controllers
{
    [ApiController]
    [Route("Api/Auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        IUserRepository _userRepository;
        IMapper _mapper;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserForRegistrationDto, User>();
                cfg.CreateMap<UserForLoginConfirmationDto, Auth>();
                cfg.CreateMap<Auth, UserForLoginConfirmationDto>();
            }));
            _authHelper = new AuthHelper(config,userRepository);
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {

            if (_userRepository.GetExistingUser(userForRegistration.Email) == null)
            {


                if (userForRegistration.Password == userForRegistration.PasswordConfirm)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);


                    Auth authDb = new Auth
                    {
                        Email = userForRegistration.Email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    };
                    _userRepository.AddEntity<Auth>(authDb);

                    User userDb = _mapper.Map<User>(userForRegistration);
                    _userRepository.AddEntity<User>(userDb);


                    if (_userRepository.SaveChanges())
                    {
                        return Ok();
                    }
                    throw new Exception("Failed to add User");
                }
                throw new Exception("User with this email already exists");
            }
            throw new Exception("Passwords do not match!");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {

            Auth authDb = _userRepository.GetAuthInfo(userForLogin.Email);

            if (authDb != null)
            {
                User userDb = _userRepository.GetExistingUser(authDb.Email);
                int userId = userDb.UserId;
                string role = userDb.Role;
                UserForLoginConfirmationDto loginConfirmation = _mapper.Map<UserForLoginConfirmationDto>(authDb);
                byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, loginConfirmation.PasswordSalt);
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != loginConfirmation.PasswordHash[i])
                    {
                        return StatusCode(404, "Incorrect Password");
                    }
                }

                return Ok(new Dictionary<string, string>{
                            {"Token",_authHelper.CreateToken(userId)}
                        });
            }
            throw new Exception("This User does not exist in the database");

        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            int.TryParse(User.FindFirst("userId")?.Value, out int userId);

            string? role = User.FindFirst("role")?.Value;
            if(role != null){

                return _authHelper.CreateToken(userId);
            }
            return _authHelper.CreateToken(userId);
        }
    }
}