﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using System.Reflection.Emit;

namespace Intercon.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(255);
        builder.Property(u => u.UserName).HasMaxLength(50);
        builder.Property(u => u.Role).HasDefaultValue(UserRole.User);
        builder.Property(u => u.AvatarId).IsRequired(false);

        builder.HasMany(u => u.Reviews)
               .WithOne(r => r.Author)
               .HasForeignKey(r => r.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Avatar)
            .WithMany()
            .HasForeignKey(u => u.AvatarId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.UserName).IsUnique();
    }
}