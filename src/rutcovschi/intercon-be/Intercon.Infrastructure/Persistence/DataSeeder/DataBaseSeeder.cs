using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
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

        using (var context = new InterconDbContext(options))
        {
            var dataBaseCreated = context.Database.EnsureCreated();

            InitializeEntity(context, context.Images, AddImages);
            InitializeEntity(context, context.UsersOld, AddUsers);
            InitializeEntity(context, context.Businesses, AddBusinesses);
            InitializeEntity(context, context.Reviews, AddReviews);

            if (dataBaseCreated)
            {
                CalculateBusinessRatings(context);
            }
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

                float reviewsSum = business.Reviews.Select(x => x.Grade).Sum();

                business.ReviewsCount = reviewsCount;
                business.Rating = reviewsSum / reviewsCount;
            }
        }

        context.SaveChanges();
    }

    private static void InitializeEntity<TEntity>(InterconDbContext context, DbSet<TEntity> entity, Func<List<TEntity>> seedAction) 
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

    private static List<User> AddUsers()
    {
        return new List<User>()
        {
            new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@mail.com",
                Password = "admin",
                UserName = "Admin",
                Role = Role.SuperAdmin
            },
            new User
            {
                Id = 2,
                FirstName = "Ion",
                LastName = "Tuc",
                Email = "ion.tuc@mail.com",
                Password = "Password",
                UserName = "ioneltuc",
                Role = Role.User
            },
            new User
            {
                Id = 3,
                FirstName = "Danu",
                LastName = "Global",
                Email = "danu.global@mail.com",
                Password = "cs2top123",
                UserName = "442God",
                Role = Role.User
            },
            new User
            {
                Id = 4,
                FirstName = "Victor",
                LastName = "Tuc",
                Email = "victor.tuc@mail.com",
                Password = "victorTuc",
                UserName = "ViTuc",
                Role = Role.User
            }
        };
    }

    private static List<Business> AddBusinesses()
    {
        return new List<Business>()
        {
            new Business
            {
                Id = 1,
                OwnerId = 1,
                Title = "Linella",
                ShortDescription = "ShortDescription",
                FullDescription = "FullDescription",
                Address = new Address(
                    Street: "str. Florilor 123",
                    Latitude: "123143",
                    Longitude: "123143"
                ),
                Category = BusinessCategory.Supermarket
            }
        };
    }

    private static List<Review> AddReviews()
    {
        return new List<Review>()
        {
            new Review
            {
                BusinessId = 1,
                AuthorId = 1,
                Grade = 5,
                ReviewText = "Mega top"
            }
        };
    }

    private static List<Image> AddImages()
    {
        return new List<Image>()
        {
            new Image
            {
                Id = 1,
                Data = "imageData"
            }
        };
    }
}
