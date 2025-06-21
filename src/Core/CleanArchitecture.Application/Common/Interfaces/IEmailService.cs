namespace CleanArchitecture.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task SendEmailAsync(string to, string subject, string body, bool isHtml, CancellationToken cancellationToken = default);
    Task SendEmailToMultipleAsync(IEnumerable<string> recipients, string subject, string body, CancellationToken cancellationToken = default);
    Task SendTemplatedEmailAsync(string to, string templateName, object model, CancellationToken cancellationToken = default);
}
