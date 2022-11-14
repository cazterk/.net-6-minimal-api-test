using ChurchSystem.Models;
public interface IUserService
{

    public User Get(UserLogin userLogin);
}