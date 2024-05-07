using Intercon.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Persistence;

public class InterconDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public InterconDbContext(DbContextOptions<InterconDbContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<FileData> DataFiles { get; set; }

    public DbSet<ReviewLike> ReviewLikes { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<ResetPasswordCode> ResetPasswordCodes { get; set; }
    public virtual DbSet<PerformanceLog> PerformanceLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
