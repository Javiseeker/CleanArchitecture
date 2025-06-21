using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;

namespace CleanArchitecture.Persistence.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly IApplicationDbContext _context;

    public TodoListRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<TodoList?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var todoList = _context.TodoLists.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(todoList);
    }

    public Task<IEnumerable<TodoList>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var todoLists = _context.TodoLists.AsEnumerable();
        return Task.FromResult(todoLists);
    }

    public async Task<TodoList> AddAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        _context.Add(todoList);
        await _context.SaveChangesAsync(cancellationToken);
        return todoList;
    }

    public async Task UpdateAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        _context.Update(todoList);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TodoList todoList, CancellationToken cancellationToken = default)
    {
        _context.Remove(todoList);
        await _context.SaveChangesAsync(cancellationToken);
    }
}