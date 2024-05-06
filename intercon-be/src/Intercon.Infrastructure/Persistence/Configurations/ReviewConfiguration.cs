using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intercon.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => new { r.BusinessId, r.AuthorId });

        builder.Property(r => r.Grade).IsRequired();
        builder.Property(r => r.ReviewText).HasMaxLength(1000);
        builder.Property(r => r.Recommendation).HasDefaultValue(RecommendationType.Neutral);

        builder.HasOne(r => r.Author)
               .WithMany(u => u.Reviews)
               .HasForeignKey(r => r.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Business)
               .WithMany(b => b.Reviews)
               .HasForeignKey(r => r.BusinessId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}