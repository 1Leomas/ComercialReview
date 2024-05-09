using Intercon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intercon.Infrastructure.Persistence.Configurations;

public class ReviewLikeConfiguration : IEntityTypeConfiguration<ReviewLike>
{
    public void Configure(EntityTypeBuilder<ReviewLike> builder)
    {
        builder.HasKey(rl => rl.Id);

        builder.HasIndex(rl => new { rl.BusinessId, rl.ReviewAuthorId, rl.UserId })
            .IsUnique();

        builder.HasOne(rl => rl.Review)
            .WithMany(r => r.Likes)
            .HasForeignKey(rl => new { rl.BusinessId, rl.ReviewAuthorId })
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rl => rl.User)
            .WithMany(u => u.ReviewLikes)
            .HasForeignKey(rl => rl.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
    public void Configure(EntityTypeBuilder<CommentLike> builder)
    {
        builder.HasKey(cl => cl.Id);

        builder.HasIndex(cl => new { cl.CommentId, cl.UserId })
            .IsUnique();

        builder.HasOne(cl => cl.Comment)
            .WithMany(c => c.Likes)
            .HasForeignKey(cl => cl.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cl => cl.User)
            .WithMany(u => u.CommentLikes)
            .HasForeignKey(cl => cl.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}