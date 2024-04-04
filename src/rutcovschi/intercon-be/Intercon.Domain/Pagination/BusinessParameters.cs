using Intercon.Domain.Enums;

namespace Intercon.Domain.Pagination;

public class BusinessParameters : QueryStringParameters
{
    public BusinessSortBy SortBy { get; set; } = BusinessSortBy.CreatedDate;
    public SortingDirection? SortDirection { get; set; }
    public uint MinGrade { get; set; }
    public int MaxGrade { get; set; } = 5;
    public string? Search { get; set; }
    public IEnumerable<BusinessCategory> Categories { get; set; } = new List<BusinessCategory>();
}