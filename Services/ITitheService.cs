using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ChurchSystem;


public interface ITitheService
{

    public Tithe Create(Tithe tithe);
    public Tithe? Get(int id);
    public List<Tithe> List();
    public Tithe? Update(Tithe tithe);
    public bool Delete(int id);



}
