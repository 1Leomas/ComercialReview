using Intercon.Domain.Entities;
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
        var dataBaseCreated = _context.Database.EnsureCreated();

        await InitializeEntity(_context.DataFiles, ImagesSeed.SeedImages);
        await InitializeUsers();
        await InitializeEntity(_context.Businesses, BusinessesSeed.SeedBusinesses);
        await InitializeEntity(_context.Reviews, ReviewsSeed.SeedReviews);

        if (dataBaseCreated)
        {
            CalculateBusinessRatings();
        }
    }

    private void CalculateBusinessRatings()
    {
        var businesses = _context.Businesses.Include(b => b.Reviews).ToList();

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
            .FirstOrDefault(p => p.PropertyType == typeof(DbSet<TEntity>));

        if (entityType == null)
        {
            throw new ArgumentException($"DbSet<{typeof(TEntity).Name}> not found in InterconDbContext.");
        }

        var tableName = entityType.Name;

        using var transaction = await _context.Database.BeginTransactionAsync();

        bool hasIdentity = HasIdentity<TEntity>(_context);

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

        await transaction.CommitAsync();
    }

    private bool HasIdentity<TEntity>(InterconDbContext context) where TEntity : class
    {
        var efEntity = context.Model.FindEntityType(typeof(TEntity));
        var efProperties = efEntity.GetProperties();

        var hasIdentity = efProperties.Any(p => (SqlServerValueGenerationStrategy)
                                                p.FindAnnotation("SqlServer:ValueGenerationStrategy").Value
                                                == SqlServerValueGenerationStrategy.IdentityColumn);

        return hasIdentity;
    }


}