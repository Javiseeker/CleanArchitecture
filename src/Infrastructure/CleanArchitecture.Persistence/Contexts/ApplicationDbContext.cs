using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Persistence.Context;

public class ApplicationDbContext : IApplicationDbContext
{
    private readonly List<TodoItem> _todoItems = new();
    private int _todoItemIdCounter = 1;

    public ApplicationDbContext()
    {
        SeedData();
    }

    public IQueryable<TodoItem> TodoItems => _todoItems.AsQueryable();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(1);
    }

    public void Add<T>(T entity) where T : class
    {
        if (entity is TodoItem todoItem)
        {
            // Use reflection to set the Id (since it's protected)
            typeof(TodoItem).GetProperty("Id")?.SetValue(todoItem, _todoItemIdCounter++);
            _todoItems.Add(todoItem);
        }
    }

    public void Update<T>(T entity) where T : class
    {
        // For in-memory, object reference is already updated
    }

    public void Remove<T>(T entity) where T : class
    {
        if (entity is TodoItem todoItem)
            _todoItems.Remove(todoItem);
    }

    private void SeedData()
    {
        var item1 = TodoItem.Create("Learn Clean Architecture", "Study the principles and patterns", Priority.High);
        var item2 = TodoItem.Create("Build TODO API", "Implement GET and POST endpoints", Priority.Medium);

        Add(item1);
        Add(item2);
    }
}