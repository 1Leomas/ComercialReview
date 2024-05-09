using Intercon.Application.Abstractions.Services;
using Intercon.Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
#pragma warning disable CA2254

namespace Intercon.Infrastructure.Services;

public class EmailService(
    IOptions<EmailSettings> emailSettings,
    ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string recipientEmail, string recipientName, string subject, string message)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, false);
        }
        catch (Exception)
        {
            _logger.LogError($"Can not connect to {_emailSettings.MailServer}:{_emailSettings.MailPort}");
            return;
        }
        try
        {
            await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);
        }
        catch (Exception)
        {
            _logger.LogError("Can not authenticate with given credentials");
            return;
        }

        var emailMessage = CreateEmailMessage(_emailSettings.Sender, recipientEmail, recipientName, subject, message);

        _logger.LogInformation($"Sending email to {recipientEmail} with subject {subject}");

        var response = await client.SendAsync(emailMessage);

        _logger.LogInformation($"Response: {response}");

        await client.DisconnectAsync(true);
    }

    private MimeMessage CreateEmailMessage(string sender, string recipient, string recipientName, string subject, string message)
    {
        var mimeMail = new MimeMessage();

        mimeMail.From.Add(new MailboxAddress(_emailSettings.SenderName, sender));
        mimeMail.To.Add(new MailboxAddress(recipientName, recipient));
        mimeMail.Subject = subject;
        mimeMail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        return mimeMail;
    }
}