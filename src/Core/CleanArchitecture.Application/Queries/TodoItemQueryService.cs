using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.Queries;

public class TodoItemQueryService : ITodoItemQueryService
{
    private readonly ITodoItemRepository _repository;

    public TodoItemQueryService(ITodoItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<TodoItem?> GetTodoItemByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}