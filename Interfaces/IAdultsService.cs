using Microsoft.AspNetCore.Mvc;
using ChurchSystem.Models;


public interface IAdultsService
{
    public Adults Create(Adults adults);
    public Adults? Get(int id);
    public List<Adults> List();
    public Adults? Update(int id, Adults adults);
}