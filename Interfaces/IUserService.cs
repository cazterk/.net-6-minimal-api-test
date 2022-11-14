using ChurchSystem.Models;
using System;
using Microsoft.AspNetCore.Mvc;
public interface IUserService

{
    public Task<ActionResult<User>> Register(User user, UserLogin request);
    public User Get(UserLogin userLogin);
}