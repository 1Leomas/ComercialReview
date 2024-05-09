using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intercon.Domain.Notifications;

public enum NotificationTypes
{
    Info = 1,
    Review = 2,
    Comment = 3,
    Like = 4,
}

[Table("NotificationType")]
public class NotificationType
{
    [Key]
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


}