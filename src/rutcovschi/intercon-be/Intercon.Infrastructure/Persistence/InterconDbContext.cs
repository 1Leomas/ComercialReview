using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Persistence;

public class InterconDbContext : DbContext
{
    public InterconDbContext(DbContextOptions options) : base(options)
    {
    }

    //public DbSet<User>
}
