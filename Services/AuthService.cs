using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;

using ChurchSystem.Models;


namespace ChurchSystem.Services
{

    using BCrypt.Net;
    public class AuthService : IAuthService
    {
        private readonly APIContext _context;

        private readonly IConfiguration _configuration;

        public AuthService(APIContext context, IConfiguration configuration)
        {

            _context = context;
            _configuration = configuration;
        }

        public async Task<User> Login(string username, string password)
        {
            User? user = await _context.User.FindAsync(username);
            if (user is null || BCrypt.Verify(password, user.Password) == false)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.GivenName, user.Name),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _configuration["jwt:Issuer"],
                Audience = _configuration["jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"])),
                      SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.IsActive = true;

            return user;
        }

        public async Task<User> Register(User user)
        {
            user.Password = BCrypt.HashPassword(user.Password);
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}