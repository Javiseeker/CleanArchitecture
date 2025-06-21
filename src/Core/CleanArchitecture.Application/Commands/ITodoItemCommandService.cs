using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Commands;

public interface ITodoItemCommandService
{
    Task<TodoItem> CreateTodoItemAsync(string title, string? description, Priority priority, DateTime? dueDate, CancellationToken cancellationToken = default);
    Task<TodoItem> UpdateTodoItemAsync(int id, string title, string? description, Priority priority, DateTime? dueDate, CancellationToken cancellationToken = default);
    Task DeleteTodoItemAsync(int id, CancellationToken cancellationToken = default);
}