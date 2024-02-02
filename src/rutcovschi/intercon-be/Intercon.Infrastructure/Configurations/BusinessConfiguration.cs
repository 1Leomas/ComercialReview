using Intercon.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intercon.Infrastructure.Configurations;

public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        // Configure the Business entity here
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(255);
        builder.Property(b => b.ShortDescription).HasMaxLength(500);
        builder.Property(b => b.FullDescription).HasMaxLength(2000);
        builder.Property(b => b.Image).HasMaxLength(255);
        builder.Property(b => b.Address).HasMaxLength(500);

        builder.HasMany(b => b.Reviews)
               .WithOne(r => r.Business)
               .HasForeignKey(r => r.BusinessId)
               .OnDelete(DeleteBehavior.Cascade); // or DeleteBehavior.Restrict if you don't want cascade delete

    }
}
