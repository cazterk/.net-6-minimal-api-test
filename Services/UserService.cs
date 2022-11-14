using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using ChurchSystem.Models;
public class UserService : IUserService
{

    private readonly APIContext _context;

    public UserService(APIContext context)
    {
        _context = context;
    }

    private void CreatePasswordHash(
        string password, out byte[] passwordHash,
        out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.GetBytes(password));

        }
    }
    public User Get(UserLogin userLogin)
    {
        User user = _context.User.FirstOrDefault(o => o.Username.Equals
        (userLogin.Username, StringComparison.OrdinalIgnoreCase) &&
        o.Password.Equals(userLogin.Password));

        return user;
    }

    public Task<ActionResult<User>> Register(User user, UserLogin request)
    {
        CreatePasswordHash(request.Password, out byte[]
        passwordHash, out byte[] passwordSalt);

        user.Username = request.Username;
        user.Password = passwordHash;
        user.PasswordSalt = passwordSalt;

        return Ok(user);
    }
}