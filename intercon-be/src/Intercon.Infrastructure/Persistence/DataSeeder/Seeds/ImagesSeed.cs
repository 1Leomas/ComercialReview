using Intercon.Domain.Entities;

namespace Intercon.Infrastructure.Persistence.DataSeeder.Seeds;

public class ImagesSeed
{
    public static List<FileData> SeedImages()
    {
        var images = new List<FileData>();

        images.AddRange(SeedBusinessLogos());

        return images;
    }

    //private static List<FileData> SeedUsersAvatars()
    //{
    //    var images = new List<FileData>();

    //    for (int i = 1; i <= 20; i++)
    //    {
    //        images.Add(new FileData
    //        {
    //            Id = i,
    //            Path = "https://interconblobstorage.blob.core.windows.net/images/6bbfd179-4c0a-4b22-91d2-eec6120e6233.jpg"
    //        });
    //    }

    //    return images;
    //}

    private static List<FileData> SeedBusinessLogos()
    {
        var images = new List<FileData>();

        images.AddRange(SeedLinellaLogos());
        images.AddRange(SeedGranierLogos());
        images.AddRange(SeedBigSportGymLogos());
        images.AddRange(SeedJungleFitnessLogos());
        images.AddRange(SeedMcDonaldsLogos());
        images.AddRange(SeedDarwinLogos());

        return images;
    }

    private static List<FileData> SeedLinellaLogos() // 1-20
    {
        var images = new List<FileData>();

        for (int i = 1; i <= 20; i++)
        {
            images.Add(new FileData
            {
                Id = i,
                Path = "https://interconblobstorage.blob.core.windows.net/images/6bbfd179-4c0a-4b22-91d2-eec6120e6231.jpg"
            });
        }

        return images;
    }
    private static List<FileData> SeedGranierLogos() // 21-28
    {
        var images = new List<FileData>();

        for (int i = 21; i <= 28; i++)
        {
            images.Add(new FileData
            {
                Id = i,
                Path = "https://interconblobstorage.blob.core.windows.net/images/6bbfd179-4c0a-4b22-91d2-eec6120e6232.png"
            });
        }

        return images;
    }

    private static List<FileData> SeedBigSportGymLogos() // 29-33
    {
        var images = new List<FileData>();

        for (int i = 29; i <= 33; i++)
        {
            images.Add(new FileData
            {
                Id = i,
                Path = "https://interconblobstorage.blob.core.windows.net/images/379e13fc-f653-46b9-83ec-59b5264fe187.png"
            });
        }

        return images;
    }

    private static List<FileData> SeedJungleFitnessLogos() // 34-34
    {
        var images = new List<FileData>();

        for (int i = 34; i <= 34; i++)
        {
            images.Add(new FileData
            {
                Id = i,
                Path = "https://interconblobstorage.blob.core.windows.net/images/379e13fc-f653-46b9-83ec-59b5264fe188.jpg"
            });
        }
        return images;
    }

    private static List<FileData> SeedMcDonaldsLogos() // 35-43
    {
        var images = new List<FileData>();

        for (int i = 35; i <= 43; i++)
        {
            images.Add(new FileData
            {
                Id = i,
                Path = "https://interconblobstorage.blob.core.windows.net/images/379e13fc-f653-46b9-83ec-59b5264fe189.png"
            });
        }
        return images;
    }

    private static List<FileData> SeedDarwinLogos() // 44-50
    {
        var images = new List<FileData>();

        for (int i = 44; i <= 50; i++)
        {
            images.Add(new FileData
            {
                Id = i,
                Path = "https://interconblobstorage.blob.core.windows.net/images/379e13fc-f653-46b9-83ec-59b5264fe186.png"
            });
        }
        return images;
    }
}
