namespace CleanArchitecture.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendTodoItemCreatedNotificationAsync(int todoItemId, string title, CancellationToken cancellationToken = default);
    Task SendTodoItemCompletedNotificationAsync(int todoItemId, string title, CancellationToken cancellationToken = default);
    Task SendTodoItemOverdueNotificationAsync(int todoItemId, string title, DateTime dueDate, CancellationToken cancellationToken = default);
}
