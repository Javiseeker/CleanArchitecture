using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Repositories;

public interface ITodoItemRepository
{
    // Basic CRUD
    Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TodoItem> AddAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(TodoItem todoItem, CancellationToken cancellationToken = default);
}