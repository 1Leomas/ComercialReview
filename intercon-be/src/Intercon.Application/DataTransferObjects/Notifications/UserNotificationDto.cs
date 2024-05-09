namespace Intercon.Application.DataTransferObjects.Notifications;

public class NotificationDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public bool IsRead { get; set; }
}