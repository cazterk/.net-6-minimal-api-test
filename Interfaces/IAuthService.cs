using ChurchSystem.Models;

public interface IAuthService
{
    public Task<User> Login(string username, string password);
    public Task<User> Register(User user);
}