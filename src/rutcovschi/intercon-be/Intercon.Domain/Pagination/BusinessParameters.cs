using Intercon.Domain.Enums;

namespace Intercon.Domain.Pagination;

public class BusinessParameters : QueryStringParameters
{
    public BusinessSortBy SortBy { get; set; } = BusinessSortBy.CreatedDate;
    public SortingDirection? SortDirection { get; set; }
    public float? Rating { get; set; }
    public string? Title { get; set; }
    public BusinessCategory? Category { get; set; }
}