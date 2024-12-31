using CShroud.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace CShroud.Infrastructure.Data;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=server;Username=crimsonshroud;Password=billieguess2Aikvbvj.kiaRH!iHOghi483ih1!89539709ih0!");
    }
}