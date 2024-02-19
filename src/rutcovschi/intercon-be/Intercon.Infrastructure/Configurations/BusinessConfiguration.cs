using Intercon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intercon.Infrastructure.Configurations;

public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(255);
        builder.Property(b => b.ShortDescription).IsRequired().HasMaxLength(500);

        builder.ComplexProperty(b => b.Address).IsRequired();

        builder.HasOne(b => b.Owner)
            .WithOne(u => u.Business)
            .HasForeignKey<Business>(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Reviews)
               .WithOne(r => r.Business)
               .HasForeignKey(r => r.BusinessId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Logo)
            .WithMany()
            .HasForeignKey(u => u.LogoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}