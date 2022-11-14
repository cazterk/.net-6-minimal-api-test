using ChurchSystem.Models;
public class UserService : IUserService
{
    private readonly APIContext _context;

    public UserService(APIContext context)
    {
        _context = context;
    }
    public User Get(UserLogin userLogin)
    {
        User user = _context.Users.FirstOrDefault(o => o.Username.Equals
        (userLogin.Username, StringComparison.OrdinalIgnoreCase) &&
        o.Password.Equals(userLogin.Password));

        return user;
    }
}