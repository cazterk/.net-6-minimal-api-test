using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using ChurchSystem.Models;


public interface IChildrenService
{

    public Children Create(Children children);
    public Children? Get(int id);
    public List<Children> List();
    public Children? Update(int id, Children children);


}