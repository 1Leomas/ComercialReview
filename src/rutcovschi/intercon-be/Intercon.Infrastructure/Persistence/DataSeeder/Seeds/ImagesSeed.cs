using Intercon.Domain.Entities;

namespace Intercon.Infrastructure.Persistence.DataSeeder.Seeds;

public class ImagesSeed
{
    public static List<FileData> SeedImages()
    {
        return new List<FileData>()
        {
            new FileData
            {
                Id = 1,
                Path = "imageData"
            }
        };
    }
}