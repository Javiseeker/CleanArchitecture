using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Persistence.Context;

public class ApplicationDbContext : IApplicationDbContext
{
    private readonly List<TodoList> _todoLists = new();
    private readonly List<TodoItem> _todoItems = new();
    private int _todoListIdCounter = 1;
    private int _todoItemIdCounter = 1;

    public ApplicationDbContext()
    {
        SeedData();
    }

    public IQueryable<TodoList> TodoLists => _todoLists.AsQueryable();
    public IQueryable<TodoItem> TodoItems => _todoItems.AsQueryable();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // In a real EF Core implementation, this would save changes to database
        // For in-memory, changes are already "saved"
        return Task.FromResult(1);
    }

    public void Add<T>(T entity) where T : class
    {
        if (entity is TodoList todoList)
        {
            todoList.GetType().GetProperty("Id")?.SetValue(todoList, _todoListIdCounter++);
            _todoLists.Add(todoList);
        }
        else if (entity is TodoItem todoItem)
        {
            todoItem.GetType().GetProperty("Id")?.SetValue(todoItem, _todoItemIdCounter++);
            _todoItems.Add(todoItem);
        }
    }

    public void Update<T>(T entity) where T : class
    {
        // For in-memory, object reference is already updated
        // In real EF Core, this would mark entity as modified
    }

    public void Remove<T>(T entity) where T : class
    {
        if (entity is TodoList todoList)
            _todoLists.Remove(todoList);
        else if (entity is TodoItem todoItem)
            _todoItems.Remove(todoItem);
    }

    private void SeedData()
    {
        var personalList = TodoList.Create("Personal Tasks", "My personal todo items");
        Add(personalList);

        var workList = TodoList.Create("Work Tasks", "Work-related todo items");
        Add(workList);
    }
}