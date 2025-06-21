using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IEmailService emailService, ILogger<NotificationService> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task SendTodoItemCreatedNotificationAsync(int todoItemId, string title, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending todo item created notification for: {TodoItemId}", todoItemId);

        // TODO: Get user email from user service
        // For now, just log
        _logger.LogInformation("Todo item created: {Title}", title);

        // Example email notification
        // await _emailService.SendEmailAsync("user@example.com", "Todo Item Created", $"Your todo item '{title}' has been created.", cancellationToken);
    }

    public async Task SendTodoItemCompletedNotificationAsync(int todoItemId, string title, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending todo item completed notification for: {TodoItemId}", todoItemId);
        _logger.LogInformation("Todo item completed: {Title}", title);
    }

    public async Task SendTodoItemOverdueNotificationAsync(int todoItemId, string title, DateTime dueDate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending todo item overdue notification for: {TodoItemId}", todoItemId);
        _logger.LogInformation("Todo item overdue: {Title}, Due: {DueDate}", title, dueDate);
    }
}