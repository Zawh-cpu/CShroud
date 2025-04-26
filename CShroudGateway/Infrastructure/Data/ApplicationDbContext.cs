


using CShroudGateway.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CShroudGateway.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<Protocol> Protocols { get; set; }
    public DbSet<Key> Keys { get; set; }
    public DbSet<Server> Servers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=server;Username=zwhnet;Password=billieguess2Aikvbvj.kiaRH!iHOghi483ih1!89539709ih0!");
    }
}