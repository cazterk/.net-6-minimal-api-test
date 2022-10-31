using ChurchSystem.Models;


public interface ITitheService
{

    public Tithe Create(Tithe tithe);
    public Tithe? Get(int id);
    public List<Tithe> List();
    public Tithe? Update(Tithe tithe);
    public bool Delete(int id);



}
