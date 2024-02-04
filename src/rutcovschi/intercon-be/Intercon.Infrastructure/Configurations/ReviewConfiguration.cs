using Intercon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intercon.Infrastructure.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Grade).IsRequired();
        builder.Property(r => r.ReviewText).HasMaxLength(1000);

        builder.HasOne(r => r.Author)
               .WithMany(u => u.Reviews)
               .HasForeignKey(r => r.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Business)
               .WithMany(b => b.Reviews)
               .HasForeignKey(r => r.BusinessId)
               .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint: One user can write only one review for a business
        builder.HasIndex(r => new { r.AuthorId, r.BusinessId }).IsUnique();
    }
}
