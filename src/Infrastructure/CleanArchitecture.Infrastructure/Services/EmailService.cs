using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        return SendEmailAsync(to, subject, body, false, cancellationToken);
    }

    public Task SendEmailAsync(string to, string subject, string body, bool isHtml, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual email sending (SendGrid, SMTP, etc.)
        _logger.LogInformation("Sending email to {To}: {Subject}", to, subject);

        // For now, just log the email
        _logger.LogInformation("Email Content: {Body}", body);

        return Task.CompletedTask;
    }

    public async Task SendEmailToMultipleAsync(IEnumerable<string> recipients, string subject, string body, CancellationToken cancellationToken = default)
    {
        var tasks = recipients.Select(recipient => SendEmailAsync(recipient, subject, body, cancellationToken));
        await Task.WhenAll(tasks);
    }

    public Task SendTemplatedEmailAsync(string to, string templateName, object model, CancellationToken cancellationToken = default)
    {
        // TODO: Implement template engine (Razor, Handlebars, etc.)
        _logger.LogInformation("Sending templated email to {To} using template {Template}", to, templateName);
        return Task.CompletedTask;
    }
}