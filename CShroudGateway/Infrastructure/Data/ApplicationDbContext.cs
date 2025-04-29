


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
}