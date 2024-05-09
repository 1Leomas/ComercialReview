namespace Intercon.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendEmailAsync(string recipientEmail, string recipientName, string subject, string message);
}