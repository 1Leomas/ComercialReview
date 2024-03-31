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


}