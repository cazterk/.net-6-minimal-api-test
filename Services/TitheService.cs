using System;
using System.Linq;
using ChurchSystem;
using ChurchSystem.Models;



public class TitheService : ITitheService
{
    private readonly APIContext _context;

    public TitheService(APIContext context)
    {
        _context = context;
    }

    public Tithe Create(Tithe tithe)
    {

        _context.Tithe.Add(tithe);
        _context.SaveChanges();

        // return CreatedAtRoute("GetTitle", new { id = tithe.Id }, tithe);
        return tithe;
    }

    public Tithe Get(int id)
    {
        var tithe = _context.Tithe.FirstOrDefault(o => o.Id == id);

        if (tithe is null) return null;

        return tithe;
    }

    public List<Tithe> List()
    {
        var tithe = _context.Tithe.ToList();
        return tithe;

    }

    public Tithe Update(int id, Tithe tithe)
    {
        var oldTithe = _context.Tithe.FirstOrDefault(o => o.Id == id);
        if (oldTithe is null) return null;

        oldTithe.CollectedAmount = tithe.CollectedAmount;
        oldTithe.MeetingType = tithe.MeetingType;

        if (oldTithe != null)
        {
            _context.SaveChanges();

        }


        return tithe;

    }

    public bool Delete(int id)
    {
        var tithe = _context.Tithe.FirstOrDefault(o => o.Id == id);
        if (tithe is null) return false;

        _context.Tithe.Remove(tithe);
        return false;

    }
}

