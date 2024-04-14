namespace Intercon.Domain.Pagination;

public class CommentParameters : QueryStringParameters
{
    public CommentSortBy SortBy { get; set; } = CommentSortBy.UpdatedDate;
    public SortingDirection? SortDirection { get; set; }
    public string? Search { get; set; }
}