using ChurchSystem;
using ChurchSystem.Models;


public class ChildrenService : IChildrenService
{
    private readonly APIContext _context;
    public ChildrenService(APIContext context)
    {
        _context = context;
    }

    public Children Create(Children children)
    {
        _context.Children.Add(children);
        _context.SaveChanges();

        return children;
    }

    public Children Get(int id)
    {
        var children = _context.Children.FirstOrDefault(o => o.Id == id);

        if (children is null) return null;
        return children;
    }

    public List<Children> List()
    {
        var children = _context.Children.ToList();
        return children;
    }

    public Children? Update(int id, Children children)
    {
        var oldChildren = _context.Children.FirstOrDefault(o => o.Id == id);
        if (oldChildren is null) return null;

        oldChildren.Brothers = children.Brothers;
        oldChildren.Sisters = children.Sisters;
        oldChildren.MeetingType = children.MeetingType;

        if (oldChildren != null)
        {
            _context.SaveChanges();
        }

        return children;

    }

}