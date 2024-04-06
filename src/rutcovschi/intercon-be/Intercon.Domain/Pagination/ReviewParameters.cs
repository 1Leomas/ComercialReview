using Intercon.Domain.Enums;

namespace Intercon.Domain.Pagination;

public class ReviewParameters : QueryStringParameters
{
    public LikeType LikeType { get; set; } = LikeType.All;
    public ReviewSortBy SortBy { get; set; } = ReviewSortBy.UpdatedDate;
    public SortingDirection? SortDirection { get; set; }
    public IEnumerable<ReviewGrade> Grades { get; set; } = new List<ReviewGrade>();
}