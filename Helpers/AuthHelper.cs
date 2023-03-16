using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cotizaciones.Data;
using Cotizaciones.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace Cotizaciones.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthHelper(IConfiguration config,IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }
        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000,
                numBytesRequested: 256 / 8
            );
        }
        public string CreateToken(int userId)
        {
            User user = _userRepository.GetSingleUser(userId);
            Claim[] claims = new Claim[]
            {
                new Claim("userId", userId.ToString()),
                new Claim("role", user.Role.ToString()),
                new Claim("name", user.FullName.ToString()),
                new Claim("avatar", user.Avatar.ToString())
            };
            string? tokenKeyString = _config.GetSection("appsettings:TokenKey").Value;
            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString != null ? tokenKeyString : ""));
            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(descriptor);


            return tokenHandler.WriteToken(token);
        }
    }
}