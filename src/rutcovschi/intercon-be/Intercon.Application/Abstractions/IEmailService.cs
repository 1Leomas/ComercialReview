namespace Intercon.Application.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string recipientEmail, string recipientName, string subject, string message);
}