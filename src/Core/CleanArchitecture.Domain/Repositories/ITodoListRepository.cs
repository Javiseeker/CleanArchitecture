using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Repositories;

public interface ITodoListRepository
{
    // Basic CRUD
    Task<TodoList?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TodoList>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TodoList> AddAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default);
    Task DeleteAsync(TodoList todoList, CancellationToken cancellationToken = default);
}