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

    private static List<User> AddUsers()
    {
        return new List<User>()
        {
            new User
            {
                Id = 1,
                FirstName = "Ally",
                LastName = "Aagaard",
                Email = "ally.aagaard@gmail.com",
                Password = "allyaagaard",
                UserName = "allyaagaard",
                Role = Role.Admin
            },
            new User
            {
                Id = 2, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Password = "johndoe123",
                UserName = "johndoe", Role = Role.Admin
            },
            new User
            {
                Id = 3, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com",
                Password = "janesmith456", UserName = "janesmith", Role = Role.Admin
            },
            new User
            {
                Id = 4, FirstName = "Michael", LastName = "Johnson", Email = "michael.johnson@example.com",
                Password = "michaeljohnson789", UserName = "michaeljohnson", Role = Role.Admin
            },
            new User
            {
                Id = 5, FirstName = "Emily", LastName = "Brown", Email = "emily.brown@example.com",
                Password = "emilybrown123", UserName = "emilybrown", Role = Role.Admin
            },
            new User
            {
                Id = 6, FirstName = "Chris", LastName = "Williams", Email = "chris.williams@example.com",
                Password = "chriswilliams456", UserName = "chriswilliams", Role = Role.Admin
            },
            new User
            {
                Id = 7, FirstName = "Ashley", LastName = "Jones", Email = "ashley.jones@example.com",
                Password = "ashleyjones789", UserName = "ashleyjones", Role = Role.Admin
            },
            new User
            {
                Id = 8, FirstName = "David", LastName = "Miller", Email = "david.miller@example.com",
                Password = "davidmiller123", UserName = "davidmiller", Role = Role.Admin
            },
            new User
            {
                Id = 9, FirstName = "Emma", LastName = "Anderson", Email = "emma.anderson@example.com",
                Password = "emmaanderson456", UserName = "emmaanderson", Role = Role.Admin
            },
            new User
            {
                Id = 10, FirstName = "Matthew", LastName = "Taylor", Email = "matthew.taylor@example.com",
                Password = "matthewtaylor789", UserName = "matthewtaylor", Role = Role.Admin
            },
            new User
            {
                Id = 11, FirstName = "Olivia", LastName = "Wilson", Email = "olivia.wilson@example.com",
                Password = "oliviawilson123", UserName = "oliviawilson", Role = Role.Admin
            },
            new User
            {
                Id = 12, FirstName = "Ryan", LastName = "Moore", Email = "ryan.moore@example.com",
                Password = "ryanmoore456", UserName = "ryanmoore", Role = Role.Admin
            },
            new User
            {
                Id = 13, FirstName = "Grace", LastName = "Martin", Email = "grace.martin@example.com",
                Password = "gracemartin789", UserName = "gracemartin", Role = Role.Admin
            },
            new User
            {
                Id = 14, FirstName = "Daniel", LastName = "White", Email = "daniel.white@example.com",
                Password = "danielwhite123", UserName = "danielwhite", Role = Role.Admin
            },
            new User
            {
                Id = 15, FirstName = "Sophia", LastName = "Harris", Email = "sophia.harris@example.com",
                Password = "sophiaharris456", UserName = "sophiaharris", Role = Role.Admin
            },
            new User
            {
                Id = 16, FirstName = "Ethan", LastName = "Clark", Email = "ethan.clark@example.com",
                Password = "ethanclark789", UserName = "ethanclark", Role = Role.Admin
            },
            new User
            {
                Id = 17, FirstName = "Ava", LastName = "Lewis", Email = "ava.lewis@example.com",
                Password = "avalewis123", UserName = "avalewis", Role = Role.Admin
            },
            new User
            {
                Id = 18, FirstName = "Andrew", LastName = "Hall", Email = "andrew.hall@example.com",
                Password = "andrewhall456", UserName = "andrewhall", Role = Role.Admin
            },
            new User
            {
                Id = 19, FirstName = "Madison", LastName = "Young", Email = "madison.young@example.com",
                Password = "madisonyoung789", UserName = "madisonyoung", Role = Role.Admin
            },
            new User
            {
                Id = 20, FirstName = "Nathan", LastName = "Cooper", Email = "nathan.cooper@example.com",
                Password = "nathancooper123", UserName = "nathancooper", Role = Role.Admin
            },
            new User
            {
                Id = 21, FirstName = "Lily", LastName = "Ward", Email = "lily.ward@example.com",
                Password = "lilyward456", UserName = "lilyward", Role = Role.Admin
            },
            new User
            {
                Id = 22, FirstName = "James", LastName = "Evans", Email = "james.evans@example.com",
                Password = "jamesevans789", UserName = "jamesevans", Role = Role.Admin
            },
            new User
            {
                Id = 23, FirstName = "Chloe", LastName = "Baker", Email = "chloe.baker@example.com",
                Password = "chloebaker123", UserName = "chloebaker", Role = Role.Admin
            },
            new User
            {
                Id = 24, FirstName = "Logan", LastName = "Adams", Email = "logan.adams@example.com",
                Password = "loganadams456", UserName = "loganadams", Role = Role.Admin
            },
            new User
            {
                Id = 25, FirstName = "Addison", LastName = "Fisher", Email = "addison.fisher@example.com",
                Password = "addisonfisher789", UserName = "addisonfisher", Role = Role.Admin
            },
            new User
            {
                Id = 26, FirstName = "Elijah", LastName = "Parker", Email = "elijah.parker@example.com",
                Password = "elijahparker123", UserName = "elijahparker", Role = Role.Admin
            },
            new User
            {
                Id = 27, FirstName = "Mia", LastName = "Graham", Email = "mia.graham@example.com",
                Password = "miagraham456", UserName = "miagraham", Role = Role.Admin
            },
            new User
            {
                Id = 28, FirstName = "Caleb", LastName = "Stone", Email = "caleb.stone@example.com",
                Password = "calebstone789", UserName = "calebstone", Role = Role.Admin
            },
            new User
            {
                Id = 29, FirstName = "Avery", LastName = "Harrison", Email = "avery.harrison@example.com",
                Password = "averyharrison123", UserName = "averyharrison", Role = Role.Admin
            },
            new User
            {
                Id = 30, FirstName = "Jackson", LastName = "Gibson", Email = "jackson.gibson@example.com",
                Password = "jacksongibson456", UserName = "jacksongibson", Role = Role.Admin
            },
            new User
            {
                Id = 31, FirstName = "Ella", LastName = "Hudson", Email = "ella.hudson@example.com",
                Password = "ellahudson789", UserName = "ellahudson", Role = Role.Admin
            },
            new User
            {
                Id = 32, FirstName = "Isaac", LastName = "Fleming", Email = "isaac.fleming@example.com",
                Password = "isaacfleming123", UserName = "isaacfleming", Role = Role.Admin
            },
            new User
            {
                Id = 33, FirstName = "Hannah", LastName = "Barnes", Email = "hannah.barnes@example.com",
                Password = "hannahbarnes456", UserName = "hannahbarnes", Role = Role.Admin
            },
            new User
            {
                Id = 34, FirstName = "Gabriel", LastName = "Wright", Email = "gabriel.wright@example.com",
                Password = "gabrielwright789", UserName = "gabrielwright", Role = Role.Admin
            },
            new User
            {
                Id = 35, FirstName = "Aria", LastName = "Cole", Email = "aria.cole@example.com",
                Password = "ariacole123", UserName = "ariacole", Role = Role.Admin
            },
            new User
            {
                Id = 36, FirstName = "Evan", LastName = "Mitchell", Email = "evan.mitchell@example.com",
                Password = "evanmitchell456", UserName = "evanmitchell", Role = Role.Admin
            },
            new User
            {
                Id = 37, FirstName = "Scarlett", LastName = "Daniels", Email = "scarlett.daniels@example.com",
                Password = "scarlettdaniels789", UserName = "scarlettdaniels", Role = Role.Admin
            },
            new User
            {
                Id = 38, FirstName = "Joseph", LastName = "Owens", Email = "joseph.owens@example.com",
                Password = "josephowens123", UserName = "josephowens", Role = Role.Admin
            },
            new User
            {
                Id = 39, FirstName = "Zoe", LastName = "Harper", Email = "zoe.harper@example.com",
                Password = "zoeharper456", UserName = "zoeharper", Role = Role.Admin
            },
            new User
            {
                Id = 40, FirstName = "Luke", LastName = "Payne", Email = "luke.payne@example.com",
                Password = "lukepayne789", UserName = "lukepayne", Role = Role.Admin
            },
            new User
            {
                Id = 41, FirstName = "Leah", LastName = "Ferguson", Email = "leah.ferguson@example.com",
                Password = "leahferguson123", UserName = "leahferguson", Role = Role.Admin
            },
            new User
            {
                Id = 42, FirstName = "Owen", LastName = "Bryant", Email = "owen.bryant@example.com",
                Password = "owenbryant456", UserName = "owenbryant", Role = Role.Admin
            },
            new User
            {
                Id = 43, FirstName = "Sofia", LastName = "Warren", Email = "sofia.warren@example.com",
                Password = "sofiawarren789", UserName = "sofiawarren", Role = Role.Admin
            },
            new User
            {
                Id = 44, FirstName = "Isaiah", LastName = "Fox", Email = "isaiah.fox@example.com",
                Password = "isaiahfox123", UserName = "isaiahfox", Role = Role.Admin
            },
            new User
            {
                Id = 45, FirstName = "Aaliyah", LastName = "Hale", Email = "aaliyah.hale@example.com",
                Password = "aaliyahhale456", UserName = "aaliyahhale", Role = Role.Admin
            },
            new User
            {
                Id = 46, FirstName = "Nicholas", LastName = "Snyder", Email = "nicholas.snyder@example.com",
                Password = "nicholassnyder789", UserName = "nicholassnyder", Role = Role.Admin
            },
            new User
            {
                Id = 47, FirstName = "Mila", LastName = "Coleman", Email = "mila.coleman@example.com",
                Password = "milacoleman123", UserName = "milacoleman", Role = Role.Admin
            },
            new User
            {
                Id = 48, FirstName = "Connor", LastName = "Baxter", Email = "connor.baxter@example.com",
                Password = "connorbaxter456", UserName = "connorbaxter", Role = Role.Admin
            },
            new User
            {
                Id = 49, FirstName = "Lillian", LastName = "Morton", Email = "lillian.morton@example.com",
                Password = "lillianmorton789", UserName = "lillianmorton", Role = Role.Admin
            },
            new User
            {
                Id = 50, FirstName = "Nicholas", LastName = "Fletcher", Email = "nicholas.fletcher@example.com",
                Password = "nicholasfletcher123", UserName = "nicholasfletcher", Role = Role.Admin
            },
            new User
            {
                Id = 51, FirstName = "Sophie", LastName = "Turner", Email = "sophie.turner@example.com",
                Password = "sophieturner123", UserName = "sophieturner", Role = Role.User
            },
            new User
            {
                Id = 52, FirstName = "Liam", LastName = "Harrison", Email = "liam.harrison@example.com",
                Password = "liamharrison456", UserName = "liamharrison", Role = Role.User
            },
            new User
            {
                Id = 53, FirstName = "Aubrey", LastName = "Rogers", Email = "aubrey.rogers@example.com",
                Password = "aubreyrogers789", UserName = "aubreyrogers", Role = Role.User
            },
            new User
            {
                Id = 54, FirstName = "Gavin", LastName = "West", Email = "gavin.west@example.com",
                Password = "gavinwest123", UserName = "gavinwest", Role = Role.User
            },
            new User
            {
                Id = 55, FirstName = "Zara", LastName = "Woods", Email = "zara.woods@example.com",
                Password = "zarawoods456", UserName = "zarawoods", Role = Role.User
            },
            new User
            {
                Id = 56, FirstName = "Tyler", LastName = "Fisher", Email = "tyler.fisher@example.com",
                Password = "tylerfisher789", UserName = "tylerfisher", Role = Role.User
            },
            new User
            {
                Id = 57, FirstName = "Aria", LastName = "Gibson", Email = "aria.gibson@example.com",
                Password = "ariagibson123", UserName = "ariagibson", Role = Role.User
            },
            new User
            {
                Id = 58, FirstName = "Oscar", LastName = "Baker", Email = "oscar.baker@example.com",
                Password = "oscarbaker456", UserName = "oscarbaker", Role = Role.User
            },
            new User
            {
                Id = 59, FirstName = "Leah", LastName = "Hill", Email = "leah.hill@example.com",
                Password = "leahhill789", UserName = "leahhill", Role = Role.User
            },
            new User
            {
                Id = 60, FirstName = "Henry", LastName = "Hudson", Email = "henry.hudson@example.com",
                Password = "henryhudson123", UserName = "henryhudson", Role = Role.User
            },
            new User
            {
                Id = 61, FirstName = "Lucy", LastName = "Keller", Email = "lucy.keller@example.com",
                Password = "lucykeller456", UserName = "lucykeller", Role = Role.User
            },
            new User
            {
                Id = 62, FirstName = "Eli", LastName = "Richards", Email = "eli.richards@example.com",
                Password = "elirichards789", UserName = "elirichards", Role = Role.User
            },
            new User
            {
                Id = 63, FirstName = "Ava", LastName = "Hansen", Email = "ava.hansen@example.com",
                Password = "avahansen123", UserName = "avahansen", Role = Role.User
            },
            new User
            {
                Id = 64, FirstName = "Mason", LastName = "Dean", Email = "mason.dean@example.com",
                Password = "masondean456", UserName = "masondean", Role = Role.User
            },
            new User
            {
                Id = 65, FirstName = "Grace", LastName = "Tucker", Email = "grace.tucker@example.com",
                Password = "gracetucker789", UserName = "gracetucker", Role = Role.User
            },
            new User
            {
                Id = 66, FirstName = "Landon", LastName = "Cole", Email = "landon.cole@example.com",
                Password = "landoncole123", UserName = "landoncole", Role = Role.User
            },
            new User
            {
                Id = 67, FirstName = "Harper", LastName = "Ray", Email = "harper.ray@example.com",
                Password = "harperray456", UserName = "harperray", Role = Role.User
            },
            new User
            {
                Id = 68, FirstName = "Sofia", LastName = "Henderson", Email = "sofia.henderson@example.com",
                Password = "sofiahenderson789", UserName = "sofiahenderson", Role = Role.User
            },
            new User
            {
                Id = 69, FirstName = "Nolan", LastName = "Barnes", Email = "nolan.barnes@example.com",
                Password = "nolanbarnes123", UserName = "nolanbarnes", Role = Role.User
            },
            new User
            {
                Id = 70, FirstName = "Scarlett", LastName = "Wells", Email = "scarlett.wells@example.com",
                Password = "scarlettwells456", UserName = "scarlettwells", Role = Role.User
            },
            new User
            {
                Id = 71, FirstName = "Gavin", LastName = "Fleming", Email = "gavin.fleming@example.com",
                Password = "gavinfleming789", UserName = "gavinfleming", Role = Role.User
            },
            new User
            {
                Id = 72, FirstName = "Madison", LastName = "Hartman", Email = "madison.hartman@example.com",
                Password = "madisonhartman123", UserName = "madisonhartman", Role = Role.User
            },
            new User
            {
                Id = 73, FirstName = "Ethan", LastName = "Russell", Email = "ethan.russell@example.com",
                Password = "ethanrussell456", UserName = "ethanrussell", Role = Role.User
            },
            new User
            {
                Id = 74, FirstName = "Zoe", LastName = "Gonzalez", Email = "zoe.gonzalez@example.com",
                Password = "zoegonzalez789", UserName = "zoegonzalez", Role = Role.User
            },
            new User
            {
                Id = 75, FirstName = "Owen", LastName = "Shaw", Email = "owen.shaw@example.com",
                Password = "owenshaw123", UserName = "owenshaw", Role = Role.User
            },
            new User
            {
                Id = 76, FirstName = "Avery", LastName = "Francis", Email = "avery.francis@example.com",
                Password = "averyfrancis456", UserName = "averyfrancis", Role = Role.User
            },
            new User
            {
                Id = 77, FirstName = "Ella", LastName = "Baldwin", Email = "ella.baldwin@example.com",
                Password = "ellabaldwin789", UserName = "ellabaldwin", Role = Role.User
            },
            new User
            {
                Id = 78, FirstName = "Jack", LastName = "Marsh", Email = "jack.marsh@example.com",
                Password = "jackmarsh123", UserName = "jackmarsh", Role = Role.User
            },
            new User
            {
                Id = 79, FirstName = "Aria", LastName = "Fisher", Email = "aria.fisher@example.com",
                Password = "ariafisher456", UserName = "ariafisher", Role = Role.User
            },
            new User
            {
                Id = 80, FirstName = "Cooper", LastName = "Hart", Email = "cooper.hart@example.com",
                Password = "cooperhart789", UserName = "cooperhart", Role = Role.User
            },
            new User
            {
                Id = 81, FirstName = "Amelia", LastName = "Ward", Email = "amelia.ward@example.com",
                Password = "ameliaward123", UserName = "ameliaward", Role = Role.User
            },
            new User
            {
                Id = 82, FirstName = "Liam", LastName = "Fox", Email = "liam.fox@example.com", Password = "liamfox456",
                UserName = "liamfox", Role = Role.User
            },
            new User
            {
                Id = 83, FirstName = "Lily", LastName = "Hansen", Email = "lily.hansen@example.com",
                Password = "lilyhansen789", UserName = "lilyhansen", Role = Role.User
            },
            new User
            {
                Id = 84, FirstName = "Logan", LastName = "Carpenter", Email = "logan.carpenter@example.com",
                Password = "logancarpenter123", UserName = "logancarpenter", Role = Role.User
            },
            new User
            {
                Id = 85, FirstName = "Hailey", LastName = "Hunter", Email = "hailey.hunter@example.com",
                Password = "haileyhunter456", UserName = "haileyhunter", Role = Role.User
            },
            new User
            {
                Id = 86, FirstName = "Carter", LastName = "Reid", Email = "carter.reid@example.com",
                Password = "carterreid789", UserName = "carterreid", Role = Role.User
            },
            new User
            {
                Id = 87, FirstName = "Chloe", LastName = "Brooks", Email = "chloe.brooks@example.com",
                Password = "chloebrooks123", UserName = "chloebrooks", Role = Role.User
            },
            new User
            {
                Id = 88, FirstName = "Mason", LastName = "Newton", Email = "mason.newton@example.com",
                Password = "masonnewton456", UserName = "masonnewton", Role = Role.User
            },
            new User
            {
                Id = 89, FirstName = "Olivia", LastName = "Wagner", Email = "olivia.wagner@example.com",
                Password = "oliviawagner789", UserName = "oliviawagner", Role = Role.User
            },
            new User
            {
                Id = 90, FirstName = "Isaac", LastName = "Harvey", Email = "isaac.harvey@example.com",
                Password = "isacharvey123", UserName = "isacharvey", Role = Role.User
            },
            new User
            {
                Id = 91, FirstName = "Zoe", LastName = "Wallace", Email = "zoe.wallace@example.com",
                Password = "zoewallace456", UserName = "zoewallace", Role = Role.User
            },
            new User
            {
                Id = 92, FirstName = "Nathan", LastName = "Bass", Email = "nathan.bass@example.com",
                Password = "nathanbass789", UserName = "nathanbass", Role = Role.User
            },
            new User
            {
                Id = 93, FirstName = "Ava", LastName = "Hess", Email = "ava.hess@example.com", Password = "avahess123",
                UserName = "avahess", Role = Role.User
            },
            new User
            {
                Id = 94, FirstName = "Caleb", LastName = "Hammond", Email = "caleb.hammond@example.com",
                Password = "calebhammond456", UserName = "calebhammond", Role = Role.User
            },
            new User
            {
                Id = 95, FirstName = "Emily", LastName = "Moss", Email = "emily.moss@example.com",
                Password = "emilymoss789", UserName = "emilymoss", Role = Role.User
            },
            new User
            {
                Id = 96, FirstName = "Ryan", LastName = "Perry", Email = "ryan.perry@example.com",
                Password = "ryanperry123", UserName = "ryanperry", Role = Role.User
            },
            new User
            {
                Id = 97, FirstName = "Sophia", LastName = "Saunders", Email = "sophia.saunders@example.com",
                Password = "sophiasaunders456", UserName = "sophiasaunders", Role = Role.User
            },
            new User
            {
                Id = 98, FirstName = "Nolan", LastName = "Stevens", Email = "nolan.stevens@example.com",
                Password = "nolanstevens789", UserName = "nolanstevens", Role = Role.User
            },
            new User
            {
                Id = 99, FirstName = "Aria", LastName = "Bishop", Email = "aria.bishop@example.com",
                Password = "ariabishop123", UserName = "ariabishop", Role = Role.User
            },
            new User
            {
                Id = 100, FirstName = "Jackson", LastName = "Leblanc", Email = "jackson.leblanc@example.com",
                Password = "jacksonleblanc456", UserName = "jacksonleblanc", Role = Role.User
            },
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
                ShortDescription =
                    "Fresh Produce Paradise: Our supermarket boasts a colorful array of crisp, farm-fresh fruits and vegetables.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Mioriţa 6, MD-2028, Chișinău, Moldova",
                    Latitude: "46.997268711131156",
                    Longitude: "28.808228905548145"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 2,
                OwnerId = 2,
                Title = "Linella",
                ShortDescription =
                    "Deli Delights: Savor the finest cuts and artisanal cheeses at our deli counter, perfect for creating gourmet sandwiches.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Mioriţa 11, MD-2028, Chișinău, Moldova",
                    Latitude: "46.99245259019449",
                    Longitude: "28.8195631373346"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 3,
                OwnerId = 3,
                Title = "Linella",
                ShortDescription =
                    "Bakery Bliss: Indulge your senses with the aroma of freshly baked bread, pastries, and cakes from our in-house bakery.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Nicolae Testemițanu 39, MD-2025, Chișinău, Moldova",
                    Latitude: "46.99033662341652",
                    Longitude: "28.827228161847135"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 4,
                OwnerId = 4,
                Title = "Linella",
                ShortDescription =
                    "International Aisles: Explore global flavors in our international section, featuring a diverse range of spices, sauces, and exotic ingredients.",
                FullDescription = "",
                Address = new Address(
                    Street: "Grenoble St 128, MD-2019, Chișinău, Moldova",
                    Latitude: "46.98159778023099",
                    Longitude: "28.838277932155503"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 5,
                OwnerId = 5,
                Title = "Linella",
                ShortDescription =
                    "Organic Oasis: Embrace a healthier lifestyle with our extensive selection of organic products, from produce to pantry staples.",
                FullDescription = "",
                Address = new Address(
                    Street: "Trajan Blvd 22, MD-2060, Chișinău, Moldova",
                    Latitude: "46.97901985761206",
                    Longitude: "28.84628902068298"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 6,
                OwnerId = 6,
                Title = "Linella",
                ShortDescription =
                    "Tech-Savvy Shopping: Enjoy a seamless experience with state-of-the-art checkout systems and contactless payment options.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Cuza Vodă 42, MD-2060, Chișinău, Moldova",
                    Latitude: "46.97440110685183",
                    Longitude: "28.8484844829509"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 7,
                OwnerId = 7,
                Title = "Linella",
                ShortDescription =
                    "Coffee Corner: Energize your shopping spree with a stop at our coffee kiosk, offering a variety of brews to suit every taste.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 44, MD-2062, Chișinău, Moldova",
                    Latitude: "46.97683767341616",
                    Longitude: "28.86946481346383"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 8,
                OwnerId = 8,
                Title = "Linella",
                ShortDescription =
                    "Budget-Friendly Buys: Discover unbeatable deals and discounts on everyday essentials throughout our aisles.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Cuza Vodă 24, MD-2072, Chișinău, Moldova",
                    Latitude: "46.98006234030243",
                    Longitude: "28.85696562791677"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 9,
                OwnerId = 9,
                Title = "Linella",
                ShortDescription =
                    "Kid-Friendly Zone: Keep the little ones entertained with a dedicated kids' corner featuring snacks, toys, and games.",
                FullDescription = "",
                Address = new Address(
                    Street: "Trajan Blvd 13/1, MD-2060, Chișinău, Moldova",
                    Latitude: "46.98254643240371",
                    Longitude: "28.852738466974355"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 10,
                OwnerId = 10,
                Title = "Linella",
                ShortDescription =
                    "Wine and Spirits Wonderland: Choose from a curated selection of wines, spirits, and craft beers to complement any occasion.",
                FullDescription = "",
                Address = new Address(
                    Street: "Independentei St 5A, Chișinău, Moldova",
                    Latitude: "46.98363535990976",
                    Longitude: "28.848487165341734"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 11,
                OwnerId = 11,
                Title = "Linella",
                ShortDescription =
                    "Health and Wellness Hub: Find a range of health-conscious products, including vitamins, supplements, and natural remedies.",
                FullDescription = "",
                Address = new Address(
                    Street: "Independentei St 4/1, MD-2043, Chișinău, Moldova",
                    Latitude: "46.9854276408486",
                    Longitude: "28.84450810863036"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 12,
                OwnerId = 12,
                Title = "Linella",
                ShortDescription =
                    "Pet Paradise: Treat your furry friends with a selection of premium pet foods, toys, and accessories.",
                FullDescription = "",
                Address = new Address(
                    Street: "Hristo Botev St 17, MD-2043, Chișinău, Moldova",
                    Latitude: "46.98837896380982",
                    Longitude: "28.847787780138415"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 13,
                OwnerId = 13,
                Title = "Linella",
                ShortDescription =
                    "Floral Fantasy: Brighten your home with fresh flowers from our floral department, offering a vibrant assortment for any occasion.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 16/1, MD-2043, Chișinău, Moldova",
                    Latitude: "46.989030778364764",
                    Longitude: "28.8523951505316"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 14,
                OwnerId = 14,
                Title = "Linella",
                ShortDescription =
                    "Bulk Bargains: Reduce waste and save money with our bulk bins filled with grains, cereals, and snacks.",
                FullDescription = "",
                Address = new Address(
                    Street: "Sarmizegetusa St 35/1, Chișinău, Moldova",
                    Latitude: "46.98675285653647",
                    Longitude: "28.873743911958265"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 15,
                OwnerId = 15,
                Title = "Linella",
                ShortDescription =
                    "Grill Masters' Haven: Elevate your BBQ game with our selection of premium meats, marinades, and grilling accessories.",
                FullDescription = "",
                Address = new Address(
                    Street: "Nicolae Titulescu St 47, MD-2032, Chișinău, Moldova",
                    Latitude: "46.99398475692658",
                    Longitude: "28.868086811800232"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 16,
                OwnerId = 16,
                Title = "Linella",
                ShortDescription =
                    "Seasonal Sensations: Experience the best of each season with rotating displays of seasonal fruits, vegetables, and decor.",
                FullDescription = "",
                Address = new Address(
                    Street: "Șoseaua Muncești 388, MD-2029, Chișinău, Moldova",
                    Latitude: "46.9840704074349",
                    Longitude: "28.89469700456433"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 17,
                OwnerId = 17,
                Title = "Linella",
                ShortDescription =
                    "Convenient Meal Solutions: Explore our ready-to-eat section for quick and delicious meal options for busy days.",
                FullDescription = "",
                Address = new Address(
                    Street: "Trandafirilor Street 13/1, MD-2038, Chișinău, Moldova",
                    Latitude: "46.99821020387124",
                    Longitude: "28.852975914683928"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 18,
                OwnerId = 18,
                Title = "Linella",
                ShortDescription =
                    "DIY Cooking Central: Unleash your inner chef with a comprehensive range of fresh ingredients and culinary tools.",
                FullDescription = "",
                Address = new Address(
                    Street: "Decebal Blvd 59, MD-2015, Chișinău, Moldova",
                    Latitude: "47.00055475652262",
                    Longitude: "28.860421333180224"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 19,
                OwnerId = 19,
                Title = "Linella",
                ShortDescription =
                    "Customer Rewards Program: Enjoy exclusive discounts and perks with our loyalty program, making every visit more rewarding.",
                FullDescription = "",
                Address = new Address(
                    Street: "Șoseaua Muncești 162a, MD-2002, Chișinău, Moldova",
                    Latitude: "47.002573085802815",
                    Longitude: "28.86912407950438"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 20,
                OwnerId = 20,
                Title = "Linella",
                ShortDescription =
                    "Environmental Initiatives: We're committed to sustainability with eco-friendly packaging and efforts to reduce our carbon footprint.",
                FullDescription = "",
                Address = new Address(
                    Street: "str. Ştefan cel Mare 6/1, MD-2001, Chișinău, Moldova",
                    Latitude: "47.014784396197186",
                    Longitude: "28.84825418047755"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 21,
                OwnerId = 21,
                Title = "Granier",
                ShortDescription =
                    "Artisanal Delights: Step into our bakery and savor the aroma of freshly baked artisanal bread, crafted with care and passion.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 23, Chișinău, Moldova",
                    Latitude: "46.987043756473525",
                    Longitude: "28.858158245545862"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 22,
                OwnerId = 22,
                Title = "Granier",
                ShortDescription =
                    "Sweet Symphony: Indulge your sweet tooth with our delectable array of pastries, cakes, and cookies, each a symphony of flavors.",
                FullDescription = "",
                Address = new Address(
                    Street: "Bulevardul Moscova 2, Chișinău, Moldova",
                    Latitude: "47.04659714748263",
                    Longitude: "28.862784138001565"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 23,
                OwnerId = 23,
                Title = "Granier",
                ShortDescription =
                    "Daily Dough Delights: Start your day right with our daily selection of freshly baked goods, from flaky croissants to hearty bagels.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ion Creangă 1/3, Chișinău, Moldova",
                    Latitude: "47.03707074148245",
                    Longitude: "28.812059282788898"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 24,
                OwnerId = 24,
                Title = "Granier",
                ShortDescription =
                    "Custom Cake Creations: Celebrate life's special moments with our custom cake service, where we turn your visions into delicious realities.",
                FullDescription = "",
                Address = new Address(
                    Street: "Ion Creanga Street 78, MD-2064, Chișinău, Moldova",
                    Latitude: "47.02794057099621",
                    Longitude: "28.794680547206337"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 25,
                OwnerId = 25,
                Title = "Granier",
                ShortDescription =
                    "Wholesome and Hearty: Embrace the goodness of our wholesome, hearty bread varieties, perfect for sandwiches or standalone enjoyment.",
                FullDescription = "",
                Address = new Address(
                    Street: "Stefan cel Mare si Sfant Boulevard 69, Chișinău, Moldova",
                    Latitude: "47.01735225708259",
                    Longitude: "28.84274959996724"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 26,
                OwnerId = 26,
                Title = "Granier",
                ShortDescription =
                    "Gourmet Gluten-Free Options: Delight in our gourmet gluten-free treats, proving that dietary restrictions shouldn't compromise flavor.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ismail 86/4, Chișinău, Moldova",
                    Latitude: "47.01704785707604",
                    Longitude: "28.848374799967235"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 27,
                OwnerId = 27,
                Title = "Granier",
                ShortDescription =
                    "Seasonal Sensations: Experience the changing seasons through our rotating menu of seasonal specialties, each a taste of something new.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 23, Chișinău, Moldova",
                    Latitude: "46.98704398518555",
                    Longitude: "28.858158246593405"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 28,
                OwnerId = 28,
                Title = "Granier",
                ShortDescription =
                    "Coffee Companions: Pair your favorite brew with our selection of muffins, scones, and Danish pastries for a delightful coffee break.",
                FullDescription = "",
                Address = new Address(
                    Street: "Alba-Iulia St 75/6, Chișinău, Moldova",
                    Latitude: "47.03817349730268",
                    Longitude: "28.769902617145586"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 29,
                OwnerId = 29,
                Title = "Big sport gym",
                ShortDescription = "Protein? We have protein. We love protein. Protein is our friend, is our life.",
                FullDescription = "",
                Address = new Address(
                    Street: "Alba-Iulia St 168, MD-2051, Chișinău, Moldova",
                    Latitude: "47.034170414465535",
                    Longitude: "28.77906807051942"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 30,
                OwnerId = 30,
                Title = "Big sport gym",
                ShortDescription =
                    "Fitness Oasis: Step into our sport gym, where health and wellness come together in a dynamic and energizing environment.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Calea Ieşilor 10, MD-2069, Chișinău, Moldova",
                    Latitude: "47.040153314542394",
                    Longitude: "28.802261799967244"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 31,
                OwnerId = 31,
                Title = "Jungle Fitness",
                ShortDescription =
                    "State-of-the-Art Equipment: Experience the latest in fitness technology with our state-of-the-art exercise machines designed for optimal results.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ceucari 2/3, Chișinău, Moldova",
                    Latitude: "47.061499693722865",
                    Longitude: "28.846700904257833"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 32,
                OwnerId = 32,
                Title = "Big sport gym",
                ShortDescription =
                    "Group Fitness Fiesta: Join our invigorating group fitness classes, from heart-pounding HIIT sessions to calming yoga, catering to all fitness levels.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Socoleni 7, MD-2020, Chișinău, Moldova",
                    Latitude: "47.061454471605394",
                    Longitude: "28.846780029415065"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 33,
                OwnerId = 33,
                Title = "Big sport gym",
                ShortDescription =
                    "Personalized Training Programs: Achieve your fitness goals with our personalized training programs, crafted by experienced fitness professionals.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ion Dumeniuc 12, MD-2075, Chișinău, Moldova",
                    Latitude: "47.05619741474855",
                    Longitude: "28.89190357051942"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 34,
                OwnerId = 34,
                Title = "Big sport gym",
                ShortDescription =
                    "Wellness Community Hub: Connect with like-minded individuals in our vibrant fitness community, fostering motivation and support.",
                FullDescription = "",
                Address = new Address(
                    Street: "Constantin Brâncuși St 3, MD-2060, Chișinău, Moldova",
                    Latitude: "46.9894162312499",
                    Longitude: "28.860072521799268"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 35,
                OwnerId = 35,
                Title = "McDonald's",
                ShortDescription =
                    "Golden Arches Haven: Enter the iconic world of McDonald's, where the unmistakable golden arches promise a familiar and delicious experience.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 21/1, MD-2038, Chișinău, Moldova",
                    Latitude: "46.98718779604783",
                    Longitude: "28.857078982756153"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 36,
                OwnerId = 36,
                Title = "McDonald's",
                ShortDescription =
                    "Fast-Food Excellence: Enjoy a quick and satisfying meal with our fast-food excellence, featuring classic favorites and innovative menu additions.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Haiducilor 31, Codru, Moldova",
                    Latitude: "46.99556232794662",
                    Longitude: "28.797881468852168"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 37,
                OwnerId = 37,
                Title = "McDonald's",
                ShortDescription =
                    "Happy Meal Magic: Delight the little ones with our famous Happy Meals, complete with a toy and a menu designed just for kids.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Arborilor 21, Chișinău, Moldova",
                    Latitude: "47.004621527706284",
                    Longitude: "28.840940282175964"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 38,
                OwnerId = 38,
                Title = "McDonald's",
                ShortDescription =
                    "Drive-Thru Convenience: Experience the ease of our drive-thru service, ensuring your favorite McDonald's meals are just a window away.",
                FullDescription = "",
                Address = new Address(
                    Street: "Stefan cel Mare si Sfant Boulevard 8, Chișinău, Moldova",
                    Latitude: "47.01666195706779",
                    Longitude: "28.84588759996724"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 39,
                OwnerId = 39,
                Title = "McDonald's",
                ShortDescription =
                    "McCafé Elegance: Elevate your coffee experience at our McCafé, offering a variety of expertly crafted coffees and delightful pastries.",
                FullDescription = "",
                Address = new Address(
                    Street: "Stefan cel Mare si Sfant Boulevard 134/1, MD-2001, Chișinău, Moldova",
                    Latitude: "47.023347857210915",
                    Longitude: "28.834795729415063"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 40,
                OwnerId = 40,
                Title = "McDonald's",
                ShortDescription =
                    "Global Flavors, Local Favorites: Explore our diverse menu that brings global flavors to local communities, ensuring a taste for every palate.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Calea Ieşilor 8, Chișinău, Moldova",
                    Latitude: "47.03967041453619",
                    Longitude: "28.80318217051942"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 41,
                OwnerId = 41,
                Title = "McDonald's",
                ShortDescription =
                    "McDelivery Magic: Enjoy the convenience of McDelivery, bringing your McDonald's favorites directly to your doorstep with just a few taps on your phone.",
                FullDescription = "",
                Address = new Address(
                    Street: "Alba-Iulia St 194/A, MD-2071, Chișinău, Moldova",
                    Latitude: "47.038700828492104",
                    Longitude: "28.770121370519423"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 42,
                OwnerId = 42,
                Title = "McDonald's",
                ShortDescription =
                    "Nutritional Transparency: Make informed choices with our nutritional information readily available, allowing you to balance taste and wellness.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Alecu Russo 2, Chișinău, Moldova",
                    Latitude: "47.04458835766566",
                    Longitude: "28.861046799967244"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 43,
                OwnerId = 43,
                Title = "McDonald's",
                ShortDescription =
                    "Ronald McDonald House Charities: Support a good cause with your meal – a portion of our proceeds goes to Ronald McDonald House Charities, helping families in need.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Mihai Sadoveanu 42/6, Chișinău, Moldova",
                    Latitude: "47.07014241492778",
                    Longitude: "28.888394929415064"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 44,
                OwnerId = 44,
                Title = "Darwin",
                ShortDescription =
                    "Tech Wonderland: Step into our electronics store, where cutting-edge gadgets and innovative devices create a tech enthusiast's paradise.",
                FullDescription = "",
                Address = new Address(
                    Street: "Independentei St 13, MD-2043, Chișinău, Moldova",
                    Latitude: "46.97775792300121",
                    Longitude: "28.85631288155769"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 45,
                OwnerId = 45,
                Title = "Darwin",
                ShortDescription =
                    "Smart Solutions Hub: Explore a world of smart solutions for your home and lifestyle, featuring the latest in connected devices and automation.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Arborilor 21, Chișinău, Moldova",
                    Latitude: "47.00430172863937",
                    Longitude: "28.840684799967242"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 46,
                OwnerId = 46,
                Title = "Darwin",
                ShortDescription =
                    "Gaming Galore: Immerse yourself in the ultimate gaming experience with our extensive collection of gaming consoles, accessories, and high-performance PCs.",
                FullDescription = "",
                Address = new Address(
                    Street: "Hincesti Hwy 60/4, MD-2028, Chișinău, Moldova",
                    Latitude: "47.00273211406181",
                    Longitude: "28.816402429415064"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 47,
                OwnerId = 47,
                Title = "Darwin",
                ShortDescription =
                    "Home Entertainment Haven: Elevate your home entertainment with state-of-the-art TVs, sound systems, and streaming devices for a cinematic experience.",
                FullDescription = "",
                Address = new Address(
                    Street: "Centru, Chișinău MD, Bulevardul Ștefan cel Mare și Sfînt 71, MD-2012, Moldova",
                    Latitude: "47.01822345710117",
                    Longitude: "28.84141332941506"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 48,
                OwnerId = 48,
                Title = "Darwin",
                ShortDescription =
                    "Mobile Marvels: Discover the latest smartphones, tablets, and wearables, offering sleek design and powerful features to keep you connected.",
                FullDescription = "",
                Address = new Address(
                    Street: "Mitropolit Varlaam St 58, Chișinău, Moldova",
                    Latitude: "47.019306871424895",
                    Longitude: "28.845023929415063"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 49,
                OwnerId = 49,
                Title = "Darwin",
                ShortDescription =
                    "Tech Accessories Alley: Enhance your devices with our wide range of accessories, from stylish phone cases to high-quality cables and chargers.",
                FullDescription = "",
                Address = new Address(
                    Street: "Stefan cel Mare si Sfant Boulevard 132, Chișinău, Moldova",
                    Latitude: "47.02248167143844",
                    Longitude: "28.836127599967234"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 50,
                OwnerId = 50,
                Title = "Darwin",
                ShortDescription =
                    "DIY Tech Projects Corner: Embark on your DIY tech projects with our selection of components, tools, and kits, empowering you to create and innovate.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ion Creangă 49/5, Сhisinau, Moldova",
                    Latitude: "47.02755544258538, ",
                    Longitude: "28.795076827982474"
                ),
                Category = BusinessCategory.Electronics
            }
        };
    }

    private static List<int> GenerateListOfRandomNumbers(int count)
    {
        Random rnd = new Random();
        List<int> randomNumbers = new List<int>();

        while (randomNumbers.Count < count)
        {
            int number = rnd.Next(1, 51);
            if (!randomNumbers.Contains(number))
            {
                randomNumbers.Add(number);
            }
        }

        return randomNumbers;
    }

    private static List<Review> AddReviews()
    {
        Random rnd = new Random();
        List<Review> seedData = new List<Review>();

        for (int businessId = 1; businessId <= 50; businessId++)
        {
            var numberOfReviews = rnd.Next(1, 21);
            var randomNumbers = GenerateListOfRandomNumbers(numberOfReviews);

            for (int i = 1; i <= numberOfReviews; i++)
            {
                seedData.Add(new Review
                {
                    BusinessId = businessId,
                    AuthorId = randomNumbers[i - 1],
                    Grade = rnd.Next(1, 6),
                    ReviewText = "Review text"
                });
            }
        }

        return seedData;
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