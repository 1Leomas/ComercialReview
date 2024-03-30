using Intercon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intercon.Infrastructure.Persistence.Configurations;

public class ResetPasswordCodeConfiguration : IEntityTypeConfiguration<ResetPasswordCode>
{
    public void Configure(EntityTypeBuilder<ResetPasswordCode> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Code).IsRequired();
        builder.Property(r => r.UserId).IsRequired();

        builder.HasOne(r => r.User)
               .WithMany()
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}