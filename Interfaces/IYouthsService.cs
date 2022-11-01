using Microsoft.AspNetCore.Mvc;
using ChurchSystem.Models;

public interface IYouthsService
{
    public Youths Create(Youths youths);
    public Youths? Get(int id);
    public List<Youths> List();
    public Youths? Update(int id, Youths youths);

}