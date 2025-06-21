using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Application.Commands;

public class TodoItemCommandService : ITodoItemCommandService
{
    private readonly ITodoItemRepository _repository;

    public TodoItemCommandService(ITodoItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<TodoItem> CreateTodoItemAsync(string title, string? description, Priority priority, DateTime? dueDate, CancellationToken cancellationToken = default)
    {
        var todoItem = TodoItem.Create(title, description, priority, dueDate);
        return await _repository.AddAsync(todoItem, cancellationToken);
    }

    public async Task<TodoItem> UpdateTodoItemAsync(int id, string title, string? description, Priority priority, DateTime? dueDate, CancellationToken cancellationToken = default)
    {
        var todoItem = await _repository.GetByIdAsync(id, cancellationToken);
        if (todoItem == null)
            throw new InvalidOperationException($"TodoItem with id {id} not found");

        todoItem.Update(title, description, priority, dueDate);
        await _repository.UpdateAsync(todoItem, cancellationToken);
        return todoItem;
    }

    public async Task DeleteTodoItemAsync(int id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }
}