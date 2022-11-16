using ChurchSystem.Models;
using System;
using Microsoft.AspNetCore.Mvc;
public interface IUserService

{
    public Task<ActionResult<User>> Register(UserLogin request);
    public User Get(UserLogin userLogin);
}