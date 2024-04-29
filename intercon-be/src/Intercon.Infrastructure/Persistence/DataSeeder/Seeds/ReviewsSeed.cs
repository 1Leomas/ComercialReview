using Intercon.Domain.Entities;

namespace Intercon.Infrastructure.Persistence.DataSeeder.Seeds;

public class ReviewsSeed
{
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

    public static List<Review> SeedReviews()
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
}