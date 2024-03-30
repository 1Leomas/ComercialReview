namespace Intercon.Infrastructure.Options;

public class EmailSettings 
{
    public string MailServer { get; set; } = null!;
    public int MailPort { get; set; }
    public string Sender { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public string Password { get; set; } = null!;
}