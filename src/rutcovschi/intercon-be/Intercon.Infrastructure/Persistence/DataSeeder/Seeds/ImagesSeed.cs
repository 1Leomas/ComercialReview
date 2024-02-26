using Intercon.Domain.Entities;

namespace Intercon.Infrastructure.Persistence.DataSeeder.Seeds;

public class ImagesSeed
{
    public static List<Image> SeedImages()
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