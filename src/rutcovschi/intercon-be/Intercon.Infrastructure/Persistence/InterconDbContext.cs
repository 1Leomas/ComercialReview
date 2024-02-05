﻿using Intercon.Domain.Entities;
using Intercon.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Persistence;

public class InterconDbContext : DbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public InterconDbContext(DbContextOptions<InterconDbContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
