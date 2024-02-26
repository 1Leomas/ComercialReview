using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Infrastructure.Persistence.DataSeeder.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Intercon.Infrastructure.Persistence.DataSeeder;

public static class DataBaseSeeder
{
    public static void Seed(string connectionString)
    {
        var options = new DbContextOptionsBuilder<InterconDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        using var context = new InterconDbContext(options);

        var dataBaseCreated = context.Database.EnsureCreated();

        InitializeEntity(context, context.Images, ImagesSeed.SeedImages);
        //InitializeEntity(context, context.AspNetUsers, UsersSeed.SeedUsers);
        InitializeEntity(context, context.Businesses, BusinessesSeed.SeedBusinesses);
        InitializeEntity(context, context.Reviews, ReviewsSeed.SeedReviews);

        if (dataBaseCreated)
        {
            CalculateBusinessRatings(context);
        }
    }

    private static void CalculateBusinessRatings(InterconDbContext context)
    {
        var businesses = context.Businesses.Include(b => b.Reviews).ToList();

        foreach (var business in businesses)
        {
            if (business.Reviews.Any())
            {
                var reviewsCount = (uint)(business.Reviews.Count());

                double reviewsSum = business.Reviews.Select(x => x.Grade).Sum();

                business.ReviewsCount = reviewsCount;
                business.Rating = (float)Math.Round(reviewsSum / reviewsCount, 1);
            }
        }

        context.SaveChanges();
    }

    private static void InitializeEntity<TEntity>(InterconDbContext context, DbSet<TEntity> entity,
        Func<List<TEntity>> seedAction)
        where TEntity : class
    {
        if (context.Set<TEntity>().Any())
        {
            return; // Database has been seeded
        }

        var entityType = typeof(InterconDbContext)
            .GetProperties()
            .FirstOrDefault(p => p.PropertyType == typeof(DbSet<TEntity>));

        if (entityType == null)
        {
            throw new ArgumentException($"DbSet<{typeof(TEntity).Name}> not found in InterconDbContext.");
        }

        var tableName = entityType.Name;

        using (var transaction = context.Database.BeginTransaction())
        {
            bool hasIdentity = HasIdentity<TEntity>(context);

            if (hasIdentity)
            {
                context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} ON");
            }

            entity.AddRange(seedAction());

            context.SaveChanges();

            if (hasIdentity)
            {
                context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} OFF");
            }

            transaction.Commit();
        }
    }

    private static bool HasIdentity<TEntity>(InterconDbContext context) where TEntity : class
    {
        var efEntity = context.Model.FindEntityType(typeof(TEntity));
        var efProperties = efEntity.GetProperties();

        var hasIdentity = efProperties.Any(p => (SqlServerValueGenerationStrategy)
                                                p.FindAnnotation("SqlServer:ValueGenerationStrategy").Value
                                                == SqlServerValueGenerationStrategy.IdentityColumn);

        return hasIdentity;
    }
}