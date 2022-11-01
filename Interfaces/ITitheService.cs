using ChurchSystem.Models;


public interface ITitheService
{
    public Tithe Create(Tithe tithe);
    public Tithe? Get(int id);
    public List<Tithe> List();
    public Tithe? Update(int id, Tithe tithe);
    public bool Delete(int id);



}
