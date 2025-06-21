using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Queries;

public interface ITodoItemQueryService
{
    Task<TodoItem?> GetTodoItemByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync(CancellationToken cancellationToken = default);
}