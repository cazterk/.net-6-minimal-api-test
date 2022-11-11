using ChurchSystem;
using ChurchSystem.Models;


public class AdultsService : IAdultsService
{
    private readonly APIContext _context;
    public AdultsService(APIContext context)
    {
        _context = context;
    }

    public Adults Create(Adults adults)
    {
        _context.Adults.Add(adults);
        _context.SaveChanges();

        return adults;
    }

    public Adults Get(int id)
    {
        var adults = _context.Adults.FirstOrDefault(o => o.Id == id);
        if (adults is null) return null;
        return adults;
    }

    public List<Adults> List()
    {
        var adults = _context.Adults.ToList();
        return adults;
    }

    public Adults? Update(int id, Adults adults)
    {
        var oldAdults = _context.Adults.FirstOrDefault(o => o.Id == id);
        if (oldAdults is null) return null;

        oldAdults.Brothers = adults.Brothers;
        oldAdults.Sisters = adults.Sisters;
        oldAdults.MeetingType = adults.MeetingType;
        oldAdults.Date = adults.Date;

        if (oldAdults != null)
        {
            _context.SaveChanges();
        }

        return adults;
    }
}