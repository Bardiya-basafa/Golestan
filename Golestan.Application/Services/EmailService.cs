namespace Golestan.Application.Services;

using Interfaces;
using Microsoft.Extensions.Logging;


public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // For production, implement actual email sending logic here
        // For development, we'll just log the email
        _logger.LogInformation($"Email to {email}, subject: {subject}, body: {htmlMessage}");
        
        // Optional: Save emails to file for debugging
        var emailsDir = Path.Combine(Directory.GetCurrentDirectory(), "sent_emails");
        Directory.CreateDirectory(emailsDir);
        var emailFile = Path.Combine(emailsDir, $"{DateTime.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid()}.html");
        await File.WriteAllTextAsync(emailFile, htmlMessage);
    }
}