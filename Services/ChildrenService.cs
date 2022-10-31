using ChurchSystem.Models;
using ChurchSystem;



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

        if (children is Children) return null;
        return children;
    }

    public List<Children> List()
    {
        var children = _context.Children.ToList();
        return children;
    }

    public Children? Update(int id)
    {
        var children = _context.Children.FirstOrDefault(o => o.Id == id);
        if (children is null) return null;

        return children;
    }

}