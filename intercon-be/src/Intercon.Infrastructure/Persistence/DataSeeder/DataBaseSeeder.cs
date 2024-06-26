﻿using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence.DataSeeder.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Intercon.Infrastructure.Persistence.DataSeeder;

public class DataBaseSeeder(InterconDbContext context, UserManager<User> userManager)
{
    private readonly InterconDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;

    public async Task Seed()
    {
        await InitializeEntity(_context.DataFiles, ImagesSeed.SeedImages);
        await InitializeUsers();
        await InitializeEntity(_context.Businesses, BusinessesSeed.SeedBusinesses);
        await InitializeEntity(_context.Reviews, ReviewsSeed.SeedReviews);

        CalculateBusinessRatings();
    }

    private void CalculateBusinessRatings()
    {
        var businesses = _context.Businesses.Include(b => b.Reviews).ToList();

        foreach (var business in businesses)
        {
            if (business.Reviews.Count == 0) continue;

            var reviewsCount = (uint)(business.Reviews.Count);

            double reviewsSum = business.Reviews.Select(x => x.Grade).Sum();

            business.ReviewsCount = reviewsCount;
            business.Rating = (float)Math.Round(reviewsSum / reviewsCount, 1);
        }

        _context.SaveChanges();
    }

    private async Task InitializeUsers()
    {
        var users = UsersSeed.SeedUsers();

        foreach (var user in users)
        {
            await _userManager.CreateAsync(user);
        }
    }

    private async Task InitializeEntity<TEntity>(DbSet<TEntity> entity,
        Func<List<TEntity>> seedAction)
        where TEntity : class
    {

        if (await _context.Set<TEntity>().AnyAsync())
        {
            return; // Database has been seeded
        }

        var entityType = typeof(InterconDbContext)
            .GetProperties()
            .FirstOrDefault(p => p.PropertyType == typeof(DbSet<TEntity>)) 
                         ?? throw new ArgumentException($"DbSet<{typeof(TEntity).Name}> not found in InterconDbContext.");

        var tableName = entityType.Name;

        using var transaction = await _context.Database.BeginTransactionAsync();

        bool hasIdentity = HasIdentity<TEntity>(_context);

#pragma warning disable EF1002
        if (hasIdentity)
        {
            await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} ON");
        }

        entity.AddRange(seedAction());

        await _context.SaveChangesAsync();

        if (hasIdentity)
        {
            await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} OFF");
        }
#pragma warning restore EF1002
        await transaction.CommitAsync();
    }

    private static bool HasIdentity<TEntity>(InterconDbContext context) where TEntity : class
    {
        try
        {
            var efEntity = context.Model.FindEntityType(typeof(TEntity));
            var efProperties = efEntity!.GetProperties();

            var hasIdentity = efProperties.Any(p =>
                (SqlServerValueGenerationStrategy)p.FindAnnotation("SqlServer:ValueGenerationStrategy")!.Value! ==
                SqlServerValueGenerationStrategy.IdentityColumn);
            return hasIdentity;

        }
        catch (Exception)
        {
            return false;
        }

    }


}