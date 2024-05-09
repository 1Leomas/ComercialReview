using Intercon.Domain.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Persistence;

public class InterconDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public InterconDbContext(DbContextOptions<InterconDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private readonly IPublisher _publisher;

    public DbSet<Business> Businesses { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<FileData> DataFiles { get; set; }

    public DbSet<ReviewLike> ReviewLikes { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<NotificationType> NotificationTypes { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<ResetPasswordCode> ResetPasswordCodes { get; set; }
    public virtual DbSet<PerformanceLog> PerformanceLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        builder.Entity<NotificationType>().HasData(
            new NotificationType() { Id = 1, TypeName = "Info", Description = "Informational notification" },
            new NotificationType() { Id = 2, TypeName = "Review", Description = "New review notification" },
            new NotificationType() { Id = 3, TypeName = "Comment", Description = "New comment notification" },
            new NotificationType() { Id = 4, TypeName = "Like", Description = "New like notification" });

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    public async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .SelectMany(e =>
            {
                var domainEvents = e.Entity.DomainEvents;

                e.Entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }
}
