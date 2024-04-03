using Intercon.Domain.Enums;

namespace Intercon.Domain.Pagination;

public class ReviewParameters : QueryStringParameters
{
    public ReviewSortBy SortBy { get; set; }
    public SortingDirection? SortDirection { get; set; }
    public IEnumerable<ReviewGrade> Grades { get; set; } = new List<ReviewGrade>();
}