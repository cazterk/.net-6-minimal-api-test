using ChurchSystem;
using ChurchSystem.Models;


public class YouthsService : IYouthsService
{

    private readonly APIContext _context;
    public YouthsService(APIContext context)
    {
        _context = context;
    }

    public Youths Create(Youths youths)
    {
        _context.Youths.Add(youths);
        _context.SaveChanges();

        return youths;
    }

    public Youths? Get(int id)
    {
        var youths = _context.Youths.FirstOrDefault(o => o.Id == id);
        if (youths is null) return null;

        return youths;
    }

    public List<Youths> List()
    {
        var youths = _context.Youths.ToList();
        return youths;
    }

    public Youths? Update(int id, Youths youths)
    {
        var oldYouths = _context.Youths.FirstOrDefault(o => o.Id == id);
        if (oldYouths is null) return null;

        oldYouths.Brothers = youths.Brothers;
        oldYouths.Sisters = youths.Sisters;
        oldYouths.MeetingType = youths.MeetingType;

        if (oldYouths != null)
        {
            _context.SaveChanges();
        }
        return youths;
    }
}