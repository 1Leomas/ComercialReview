using Intercon.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Persistence;

public class InterconDbContext : DbContext
{
    public InterconDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Business> Businesses { get; set; }

    // to do -> apply model configurations
    // create entity configurations
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(InterconDbContext).Assembly);
    }
}
