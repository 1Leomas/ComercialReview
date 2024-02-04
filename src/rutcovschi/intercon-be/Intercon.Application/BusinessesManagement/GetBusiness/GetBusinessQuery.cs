using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain.ComplexTypes;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.GetBusiness;

public sealed record GetBusinessQuery(int Id) : IQuery<BusinessDetailsDto?>;

public sealed class GetBusinessQueryHandler(InterconDbContext context) : IQueryHandler<GetBusinessQuery, BusinessDetailsDto?>
{
    public async Task<BusinessDetailsDto?> Handle(GetBusinessQuery query, CancellationToken cancellationToken)
    {
        var business = await context.Businesses
            .Include(business => business.Logo)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (business == null)
        {
            return null;
        }

        return new BusinessDetailsDto()
        {
            Id = business.Id,
            Title = business.Title,
            ShortDescription = business.ShortDescription,
            FullDescription = business.FullDescription,
            Rating = business.Rating,
            Address = business.Address,
            ReviewsCount = business.ReviewsCount
        };
    }
}

public record BusinessDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? FullDescription { get; set; } = string.Empty;
    public float Rating { get; set; }
    public Address Address { get; set; } = null!;
    public uint ReviewsCount { get; set; }
}
