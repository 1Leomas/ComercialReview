using Intercon.Domain.Entities;
using Intercon.Domain.Enums;

namespace Intercon.Infrastructure.Persistence.DataSeeder.Seeds;

public class UsersSeed
{
    public static List<User> SeedUsers()
    {
        return new List<User>()
        {
            new User
            {
                Id = 1,
                FirstName = "Ally",
                LastName = "Aagaard",
                Email = "ally.aagaard@gmail.com",
                PasswordHash = "allyaagaard",
                UserName = "allyaagaard",
                Role = Role.Admin
            },
            new User
            {
                Id = 2, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com",
                PasswordHash = "johndoe123",
                UserName = "johndoe", Role = Role.Admin
            },
            new User
            {
                Id = 3, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com",
                PasswordHash = "janesmith456", UserName = "janesmith", Role = Role.Admin
            },
            new User
            {
                Id = 4, FirstName = "Michael", LastName = "Johnson", Email = "michael.johnson@example.com",
                PasswordHash = "michaeljohnson789", UserName = "michaeljohnson", Role = Role.Admin
            },
            new User
            {
                Id = 5, FirstName = "Emily", LastName = "Brown", Email = "emily.brown@example.com",
                PasswordHash = "emilybrown123", UserName = "emilybrown", Role = Role.Admin
            },
            new User
            {
                Id = 6, FirstName = "Chris", LastName = "Williams", Email = "chris.williams@example.com",
                PasswordHash = "chriswilliams456", UserName = "chriswilliams", Role = Role.Admin
            },
            new User
            {
                Id = 7, FirstName = "Ashley", LastName = "Jones", Email = "ashley.jones@example.com",
                PasswordHash = "ashleyjones789", UserName = "ashleyjones", Role = Role.Admin
            },
            new User
            {
                Id = 8, FirstName = "David", LastName = "Miller", Email = "david.miller@example.com",
                PasswordHash = "davidmiller123", UserName = "davidmiller", Role = Role.Admin
            },
            new User
            {
                Id = 9, FirstName = "Emma", LastName = "Anderson", Email = "emma.anderson@example.com",
                PasswordHash = "emmaanderson456", UserName = "emmaanderson", Role = Role.Admin
            },
            new User
            {
                Id = 10, FirstName = "Matthew", LastName = "Taylor", Email = "matthew.taylor@example.com",
                PasswordHash = "matthewtaylor789", UserName = "matthewtaylor", Role = Role.Admin
            },
            new User
            {
                Id = 11, FirstName = "Olivia", LastName = "Wilson", Email = "olivia.wilson@example.com",
                PasswordHash = "oliviawilson123", UserName = "oliviawilson", Role = Role.Admin
            },
            new User
            {
                Id = 12, FirstName = "Ryan", LastName = "Moore", Email = "ryan.moore@example.com",
                PasswordHash = "ryanmoore456", UserName = "ryanmoore", Role = Role.Admin
            },
            new User
            {
                Id = 13, FirstName = "Grace", LastName = "Martin", Email = "grace.martin@example.com",
                PasswordHash = "gracemartin789", UserName = "gracemartin", Role = Role.Admin
            },
            new User
            {
                Id = 14, FirstName = "Daniel", LastName = "White", Email = "daniel.white@example.com",
                PasswordHash = "danielwhite123", UserName = "danielwhite", Role = Role.Admin
            },
            new User
            {
                Id = 15, FirstName = "Sophia", LastName = "Harris", Email = "sophia.harris@example.com",
                PasswordHash = "sophiaharris456", UserName = "sophiaharris", Role = Role.Admin
            },
            new User
            {
                Id = 16, FirstName = "Ethan", LastName = "Clark", Email = "ethan.clark@example.com",
                PasswordHash = "ethanclark789", UserName = "ethanclark", Role = Role.Admin
            },
            new User
            {
                Id = 17, FirstName = "Ava", LastName = "Lewis", Email = "ava.lewis@example.com",
                PasswordHash = "avalewis123", UserName = "avalewis", Role = Role.Admin
            },
            new User
            {
                Id = 18, FirstName = "Andrew", LastName = "Hall", Email = "andrew.hall@example.com",
                PasswordHash = "andrewhall456", UserName = "andrewhall", Role = Role.Admin
            },
            new User
            {
                Id = 19, FirstName = "Madison", LastName = "Young", Email = "madison.young@example.com",
                PasswordHash = "madisonyoung789", UserName = "madisonyoung", Role = Role.Admin
            },
            new User
            {
                Id = 20, FirstName = "Nathan", LastName = "Cooper", Email = "nathan.cooper@example.com",
                PasswordHash = "nathancooper123", UserName = "nathancooper", Role = Role.Admin
            },
            new User
            {
                Id = 21, FirstName = "Lily", LastName = "Ward", Email = "lily.ward@example.com",
                PasswordHash = "lilyward456", UserName = "lilyward", Role = Role.Admin
            },
            new User
            {
                Id = 22, FirstName = "James", LastName = "Evans", Email = "james.evans@example.com",
                PasswordHash = "jamesevans789", UserName = "jamesevans", Role = Role.Admin
            },
            new User
            {
                Id = 23, FirstName = "Chloe", LastName = "Baker", Email = "chloe.baker@example.com",
                PasswordHash = "chloebaker123", UserName = "chloebaker", Role = Role.Admin
            },
            new User
            {
                Id = 24, FirstName = "Logan", LastName = "Adams", Email = "logan.adams@example.com",
                PasswordHash = "loganadams456", UserName = "loganadams", Role = Role.Admin
            },
            new User
            {
                Id = 25, FirstName = "Addison", LastName = "Fisher", Email = "addison.fisher@example.com",
                PasswordHash = "addisonfisher789", UserName = "addisonfisher", Role = Role.Admin
            },
            new User
            {
                Id = 26, FirstName = "Elijah", LastName = "Parker", Email = "elijah.parker@example.com",
                PasswordHash = "elijahparker123", UserName = "elijahparker", Role = Role.Admin
            },
            new User
            {
                Id = 27, FirstName = "Mia", LastName = "Graham", Email = "mia.graham@example.com",
                PasswordHash = "miagraham456", UserName = "miagraham", Role = Role.Admin
            },
            new User
            {
                Id = 28, FirstName = "Caleb", LastName = "Stone", Email = "caleb.stone@example.com",
                PasswordHash = "calebstone789", UserName = "calebstone", Role = Role.Admin
            },
            new User
            {
                Id = 29, FirstName = "Avery", LastName = "Harrison", Email = "avery.harrison@example.com",
                PasswordHash = "averyharrison123", UserName = "averyharrison", Role = Role.Admin
            },
            new User
            {
                Id = 30, FirstName = "Jackson", LastName = "Gibson", Email = "jackson.gibson@example.com",
                PasswordHash = "jacksongibson456", UserName = "jacksongibson", Role = Role.Admin
            },
            new User
            {
                Id = 31, FirstName = "Ella", LastName = "Hudson", Email = "ella.hudson@example.com",
                PasswordHash = "ellahudson789", UserName = "ellahudson", Role = Role.Admin
            },
            new User
            {
                Id = 32, FirstName = "Isaac", LastName = "Fleming", Email = "isaac.fleming@example.com",
                PasswordHash = "isaacfleming123", UserName = "isaacfleming", Role = Role.Admin
            },
            new User
            {
                Id = 33, FirstName = "Hannah", LastName = "Barnes", Email = "hannah.barnes@example.com",
                PasswordHash = "hannahbarnes456", UserName = "hannahbarnes", Role = Role.Admin
            },
            new User
            {
                Id = 34, FirstName = "Gabriel", LastName = "Wright", Email = "gabriel.wright@example.com",
                PasswordHash = "gabrielwright789", UserName = "gabrielwright", Role = Role.Admin
            },
            new User
            {
                Id = 35, FirstName = "Aria", LastName = "Cole", Email = "aria.cole@example.com",
                PasswordHash = "ariacole123", UserName = "ariacole", Role = Role.Admin
            },
            new User
            {
                Id = 36, FirstName = "Evan", LastName = "Mitchell", Email = "evan.mitchell@example.com",
                PasswordHash = "evanmitchell456", UserName = "evanmitchell", Role = Role.Admin
            },
            new User
            {
                Id = 37, FirstName = "Scarlett", LastName = "Daniels", Email = "scarlett.daniels@example.com",
                PasswordHash = "scarlettdaniels789", UserName = "scarlettdaniels", Role = Role.Admin
            },
            new User
            {
                Id = 38, FirstName = "Joseph", LastName = "Owens", Email = "joseph.owens@example.com",
                PasswordHash = "josephowens123", UserName = "josephowens", Role = Role.Admin
            },
            new User
            {
                Id = 39, FirstName = "Zoe", LastName = "Harper", Email = "zoe.harper@example.com",
                PasswordHash = "zoeharper456", UserName = "zoeharper", Role = Role.Admin
            },
            new User
            {
                Id = 40, FirstName = "Luke", LastName = "Payne", Email = "luke.payne@example.com",
                PasswordHash = "lukepayne789", UserName = "lukepayne", Role = Role.Admin
            },
            new User
            {
                Id = 41, FirstName = "Leah", LastName = "Ferguson", Email = "leah.ferguson@example.com",
                PasswordHash = "leahferguson123", UserName = "leahferguson", Role = Role.Admin
            },
            new User
            {
                Id = 42, FirstName = "Owen", LastName = "Bryant", Email = "owen.bryant@example.com",
                PasswordHash = "owenbryant456", UserName = "owenbryant", Role = Role.Admin
            },
            new User
            {
                Id = 43, FirstName = "Sofia", LastName = "Warren", Email = "sofia.warren@example.com",
                PasswordHash = "sofiawarren789", UserName = "sofiawarren", Role = Role.Admin
            },
            new User
            {
                Id = 44, FirstName = "Isaiah", LastName = "Fox", Email = "isaiah.fox@example.com",
                PasswordHash = "isaiahfox123", UserName = "isaiahfox", Role = Role.Admin
            },
            new User
            {
                Id = 45, FirstName = "Aaliyah", LastName = "Hale", Email = "aaliyah.hale@example.com",
                PasswordHash = "aaliyahhale456", UserName = "aaliyahhale", Role = Role.Admin
            },
            new User
            {
                Id = 46, FirstName = "Nicholas", LastName = "Snyder", Email = "nicholas.snyder@example.com",
                PasswordHash = "nicholassnyder789", UserName = "nicholassnyder", Role = Role.Admin
            },
            new User
            {
                Id = 47, FirstName = "Mila", LastName = "Coleman", Email = "mila.coleman@example.com",
                PasswordHash = "milacoleman123", UserName = "milacoleman", Role = Role.Admin
            },
            new User
            {
                Id = 48, FirstName = "Connor", LastName = "Baxter", Email = "connor.baxter@example.com",
                PasswordHash = "connorbaxter456", UserName = "connorbaxter", Role = Role.Admin
            },
            new User
            {
                Id = 49, FirstName = "Lillian", LastName = "Morton", Email = "lillian.morton@example.com",
                PasswordHash = "lillianmorton789", UserName = "lillianmorton", Role = Role.Admin
            },
            new User
            {
                Id = 50, FirstName = "Nicholas", LastName = "Fletcher", Email = "nicholas.fletcher@example.com",
                PasswordHash = "nicholasfletcher123", UserName = "nicholasfletcher", Role = Role.Admin
            },
            new User
            {
                Id = 51, FirstName = "Sophie", LastName = "Turner", Email = "sophie.turner@example.com",
                PasswordHash = "sophieturner123", UserName = "sophieturner", Role = Role.User
            },
            new User
            {
                Id = 52, FirstName = "Liam", LastName = "Harrison", Email = "liam.harrison@example.com",
                PasswordHash = "liamharrison456", UserName = "liamharrison", Role = Role.User
            },
            new User
            {
                Id = 53, FirstName = "Aubrey", LastName = "Rogers", Email = "aubrey.rogers@example.com",
                PasswordHash = "aubreyrogers789", UserName = "aubreyrogers", Role = Role.User
            },
            new User
            {
                Id = 54, FirstName = "Gavin", LastName = "West", Email = "gavin.west@example.com",
                PasswordHash = "gavinwest123", UserName = "gavinwest", Role = Role.User
            },
            new User
            {
                Id = 55, FirstName = "Zara", LastName = "Woods", Email = "zara.woods@example.com",
                PasswordHash = "zarawoods456", UserName = "zarawoods", Role = Role.User
            },
            new User
            {
                Id = 56, FirstName = "Tyler", LastName = "Fisher", Email = "tyler.fisher@example.com",
                PasswordHash = "tylerfisher789", UserName = "tylerfisher", Role = Role.User
            },
            new User
            {
                Id = 57, FirstName = "Aria", LastName = "Gibson", Email = "aria.gibson@example.com",
                PasswordHash = "ariagibson123", UserName = "ariagibson", Role = Role.User
            },
            new User
            {
                Id = 58, FirstName = "Oscar", LastName = "Baker", Email = "oscar.baker@example.com",
                PasswordHash = "oscarbaker456", UserName = "oscarbaker", Role = Role.User
            },
            new User
            {
                Id = 59, FirstName = "Leah", LastName = "Hill", Email = "leah.hill@example.com",
                PasswordHash = "leahhill789", UserName = "leahhill", Role = Role.User
            },
            new User
            {
                Id = 60, FirstName = "Henry", LastName = "Hudson", Email = "henry.hudson@example.com",
                PasswordHash = "henryhudson123", UserName = "henryhudson", Role = Role.User
            },
            new User
            {
                Id = 61, FirstName = "Lucy", LastName = "Keller", Email = "lucy.keller@example.com",
                PasswordHash = "lucykeller456", UserName = "lucykeller", Role = Role.User
            },
            new User
            {
                Id = 62, FirstName = "Eli", LastName = "Richards", Email = "eli.richards@example.com",
                PasswordHash = "elirichards789", UserName = "elirichards", Role = Role.User
            },
            new User
            {
                Id = 63, FirstName = "Ava", LastName = "Hansen", Email = "ava.hansen@example.com",
                PasswordHash = "avahansen123", UserName = "avahansen", Role = Role.User
            },
            new User
            {
                Id = 64, FirstName = "Mason", LastName = "Dean", Email = "mason.dean@example.com",
                PasswordHash = "masondean456", UserName = "masondean", Role = Role.User
            },
            new User
            {
                Id = 65, FirstName = "Grace", LastName = "Tucker", Email = "grace.tucker@example.com",
                PasswordHash = "gracetucker789", UserName = "gracetucker", Role = Role.User
            },
            new User
            {
                Id = 66, FirstName = "Landon", LastName = "Cole", Email = "landon.cole@example.com",
                PasswordHash = "landoncole123", UserName = "landoncole", Role = Role.User
            },
            new User
            {
                Id = 67, FirstName = "Harper", LastName = "Ray", Email = "harper.ray@example.com",
                PasswordHash = "harperray456", UserName = "harperray", Role = Role.User
            },
            new User
            {
                Id = 68, FirstName = "Sofia", LastName = "Henderson", Email = "sofia.henderson@example.com",
                PasswordHash = "sofiahenderson789", UserName = "sofiahenderson", Role = Role.User
            },
            new User
            {
                Id = 69, FirstName = "Nolan", LastName = "Barnes", Email = "nolan.barnes@example.com",
                PasswordHash = "nolanbarnes123", UserName = "nolanbarnes", Role = Role.User
            },
            new User
            {
                Id = 70, FirstName = "Scarlett", LastName = "Wells", Email = "scarlett.wells@example.com",
                PasswordHash = "scarlettwells456", UserName = "scarlettwells", Role = Role.User
            },
            new User
            {
                Id = 71, FirstName = "Gavin", LastName = "Fleming", Email = "gavin.fleming@example.com",
                PasswordHash = "gavinfleming789", UserName = "gavinfleming", Role = Role.User
            },
            new User
            {
                Id = 72, FirstName = "Madison", LastName = "Hartman", Email = "madison.hartman@example.com",
                PasswordHash = "madisonhartman123", UserName = "madisonhartman", Role = Role.User
            },
            new User
            {
                Id = 73, FirstName = "Ethan", LastName = "Russell", Email = "ethan.russell@example.com",
                PasswordHash = "ethanrussell456", UserName = "ethanrussell", Role = Role.User
            },
            new User
            {
                Id = 74, FirstName = "Zoe", LastName = "Gonzalez", Email = "zoe.gonzalez@example.com",
                PasswordHash = "zoegonzalez789", UserName = "zoegonzalez", Role = Role.User
            },
            new User
            {
                Id = 75, FirstName = "Owen", LastName = "Shaw", Email = "owen.shaw@example.com",
                PasswordHash = "owenshaw123", UserName = "owenshaw", Role = Role.User
            },
            new User
            {
                Id = 76, FirstName = "Avery", LastName = "Francis", Email = "avery.francis@example.com",
                PasswordHash = "averyfrancis456", UserName = "averyfrancis", Role = Role.User
            },
            new User
            {
                Id = 77, FirstName = "Ella", LastName = "Baldwin", Email = "ella.baldwin@example.com",
                PasswordHash = "ellabaldwin789", UserName = "ellabaldwin", Role = Role.User
            },
            new User
            {
                Id = 78, FirstName = "Jack", LastName = "Marsh", Email = "jack.marsh@example.com",
                PasswordHash = "jackmarsh123", UserName = "jackmarsh", Role = Role.User
            },
            new User
            {
                Id = 79, FirstName = "Aria", LastName = "Fisher", Email = "aria.fisher@example.com",
                PasswordHash = "ariafisher456", UserName = "ariafisher", Role = Role.User
            },
            new User
            {
                Id = 80, FirstName = "Cooper", LastName = "Hart", Email = "cooper.hart@example.com",
                PasswordHash = "cooperhart789", UserName = "cooperhart", Role = Role.User
            },
            new User
            {
                Id = 81, FirstName = "Amelia", LastName = "Ward", Email = "amelia.ward@example.com",
                PasswordHash = "ameliaward123", UserName = "ameliaward", Role = Role.User
            },
            new User
            {
                Id = 82, FirstName = "Liam", LastName = "Fox", Email = "liam.fox@example.com",
                PasswordHash = "liamfox456",
                UserName = "liamfox", Role = Role.User
            },
            new User
            {
                Id = 83, FirstName = "Lily", LastName = "Hansen", Email = "lily.hansen@example.com",
                PasswordHash = "lilyhansen789", UserName = "lilyhansen", Role = Role.User
            },
            new User
            {
                Id = 84, FirstName = "Logan", LastName = "Carpenter", Email = "logan.carpenter@example.com",
                PasswordHash = "logancarpenter123", UserName = "logancarpenter", Role = Role.User
            },
            new User
            {
                Id = 85, FirstName = "Hailey", LastName = "Hunter", Email = "hailey.hunter@example.com",
                PasswordHash = "haileyhunter456", UserName = "haileyhunter", Role = Role.User
            },
            new User
            {
                Id = 86, FirstName = "Carter", LastName = "Reid", Email = "carter.reid@example.com",
                PasswordHash = "carterreid789", UserName = "carterreid", Role = Role.User
            },
            new User
            {
                Id = 87, FirstName = "Chloe", LastName = "Brooks", Email = "chloe.brooks@example.com",
                PasswordHash = "chloebrooks123", UserName = "chloebrooks", Role = Role.User
            },
            new User
            {
                Id = 88, FirstName = "Mason", LastName = "Newton", Email = "mason.newton@example.com",
                PasswordHash = "masonnewton456", UserName = "masonnewton", Role = Role.User
            },
            new User
            {
                Id = 89, FirstName = "Olivia", LastName = "Wagner", Email = "olivia.wagner@example.com",
                PasswordHash = "oliviawagner789", UserName = "oliviawagner", Role = Role.User
            },
            new User
            {
                Id = 90, FirstName = "Isaac", LastName = "Harvey", Email = "isaac.harvey@example.com",
                PasswordHash = "isacharvey123", UserName = "isacharvey", Role = Role.User
            },
            new User
            {
                Id = 91, FirstName = "Zoe", LastName = "Wallace", Email = "zoe.wallace@example.com",
                PasswordHash = "zoewallace456", UserName = "zoewallace", Role = Role.User
            },
            new User
            {
                Id = 92, FirstName = "Nathan", LastName = "Bass", Email = "nathan.bass@example.com",
                PasswordHash = "nathanbass789", UserName = "nathanbass", Role = Role.User
            },
            new User
            {
                Id = 93, FirstName = "Ava", LastName = "Hess", Email = "ava.hess@example.com",
                PasswordHash = "avahess123",
                UserName = "avahess", Role = Role.User
            },
            new User
            {
                Id = 94, FirstName = "Caleb", LastName = "Hammond", Email = "caleb.hammond@example.com",
                PasswordHash = "calebhammond456", UserName = "calebhammond", Role = Role.User
            },
            new User
            {
                Id = 95, FirstName = "Emily", LastName = "Moss", Email = "emily.moss@example.com",
                PasswordHash = "emilymoss789", UserName = "emilymoss", Role = Role.User
            },
            new User
            {
                Id = 96, FirstName = "Ryan", LastName = "Perry", Email = "ryan.perry@example.com",
                PasswordHash = "ryanperry123", UserName = "ryanperry", Role = Role.User
            },
            new User
            {
                Id = 97, FirstName = "Sophia", LastName = "Saunders", Email = "sophia.saunders@example.com",
                PasswordHash = "sophiasaunders456", UserName = "sophiasaunders", Role = Role.User
            },
            new User
            {
                Id = 98, FirstName = "Nolan", LastName = "Stevens", Email = "nolan.stevens@example.com",
                PasswordHash = "nolanstevens789", UserName = "nolanstevens", Role = Role.User
            },
            new User
            {
                Id = 99, FirstName = "Aria", LastName = "Bishop", Email = "aria.bishop@example.com",
                PasswordHash = "ariabishop123", UserName = "ariabishop", Role = Role.User
            },
            new User
            {
                Id = 100, FirstName = "Jackson", LastName = "Leblanc", Email = "jackson.leblanc@example.com",
                PasswordHash = "jacksonleblanc456", UserName = "jacksonleblanc", Role = Role.User
            },
        };
    }
}