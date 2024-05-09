using System.ComponentModel.DataAnnotations;

namespace Intercon.Domain.Notifications;

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;

    [Required]
    public virtual int NotificationTypeId
    {
        get => (int)this.NotificationType;
        set => NotificationType = (NotificationTypes)value;
    }
    [EnumDataType(typeof(NotificationTypes))]
    public NotificationTypes NotificationType { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsRead { get; set; }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}