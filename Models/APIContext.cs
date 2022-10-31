using Microsoft.EntityFrameworkCore;
using ChurchSystem;
namespace ChurchSystem.Models

{

    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        public DbSet<Youths> Youths => Set<Youths>();
        public DbSet<Children> Children => Set<Children>();
        public DbSet<Tithe> Tithe => Set<Tithe>();
    }
}