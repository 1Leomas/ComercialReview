namespace Intercon.Domain.Pagination;

public class PerformanceLogsParameters : LogsQueryStringParameters
{
    public PerformanceLogsSortBy SortBy { get; set; } = PerformanceLogsSortBy.StartTime;
    public SortingDirection? SortDirection { get; set; }
    public string? Search { get; set; }
    public bool? IsSuccess { get; set; } = null;
}

public enum PerformanceLogsSortBy
{
    StartTime = 0,
    RequestDuration = 1,
    RequestName = 2,
}