namespace Intercon.Domain.Pagination;

public class PerformanceLogsParameters : QueryStringParameters
{
    public PerformanceLogsSortBy SortBy { get; set; } = PerformanceLogsSortBy.StartTime;
    public SortingDirection? SortDirection { get; set; }
    public string? Search { get; set; }
}

public enum PerformanceLogsSortBy
{
    StartTime = 0,
    RequestDuration = 1,
    RequestName = 2,
}