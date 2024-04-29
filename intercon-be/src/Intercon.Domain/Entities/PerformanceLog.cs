namespace Intercon.Domain.Entities;

public class PerformanceLog
{
    public int Id { get; set; }
    public string RequestName { get; set; } = string.Empty;
    public TimeSpan RequestDuration { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}