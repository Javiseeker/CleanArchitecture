using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Persistence.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly IApplicationDbContext _context;

    public TodoItemRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> AddAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        _context.Add(todoItem);
        await _context.SaveChangesAsync(cancellationToken);
        return todoItem;
    }

    public Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var todoItem = _context.TodoItems.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(todoItem);
    }

    public Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var todoItems = _context.TodoItems.AsEnumerable();
        return Task.FromResult(todoItems);
    }

    public Task<IEnumerable<TodoItem>> GetByTodoListIdAsync(int todoListId, CancellationToken cancellationToken = default)
    {
        var todoItems = _context.TodoItems.Where(x => x.TodoListId == todoListId);
        return Task.FromResult(todoItems.AsEnumerable());
    }

    public async Task UpdateAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        _context.Update(todoItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TodoItem todoItem, CancellationToken cancellationToken = default)
    {
        _context.Remove(todoItem);
        await _context.SaveChangesAsync(cancellationToken);
    }
}