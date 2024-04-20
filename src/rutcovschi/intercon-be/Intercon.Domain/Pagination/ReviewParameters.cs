using Intercon.Domain.Enums;

namespace Intercon.Domain.Pagination;

public class ReviewParameters : QueryStringParameters
{
    public RecommendationType RecommendationType { get; set; } = RecommendationType.Neutral;
    public ReviewSortBy SortBy { get; set; } = ReviewSortBy.UpdatedDate;
    public SortingDirection? SortDirection { get; set; }
    public IEnumerable<ReviewGrade> Grades { get; set; } = new List<ReviewGrade>();
    public string? Search { get; set; }
}