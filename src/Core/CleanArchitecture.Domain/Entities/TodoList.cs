using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Events;

namespace CleanArchitecture.Domain.Entities;

public class TodoList : BaseEntity<int>, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsArchived { get; private set; }
    private readonly List<TodoItem> _todoItems = new();

    public IReadOnlyCollection<TodoItem> TodoItems => _todoItems.AsReadOnly();

    // Calculated properties
    public int TotalItems => _todoItems.Count;
    public int CompletedItems => _todoItems.Count(x => x.IsCompleted);
    public int PendingItems => _todoItems.Count(x => !x.IsCompleted && !x.IsCancelled);
    public int OverdueItems => _todoItems.Count(x => x.IsOverdue);
    public decimal CompletionPercentage => TotalItems == 0 ? 0 : (decimal)CompletedItems / TotalItems * 100;

    private TodoList() { } // For EF Core

    private TodoList(string name, string? description = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        IsArchived = false;

        AddDomainEvent(new TodoListCreatedEvent(Id, name));
    }

    public static TodoList Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Todo list name cannot be empty", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Todo list name cannot exceed 100 characters", nameof(name));

        return new TodoList(name, description);
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Todo list name cannot be empty", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Todo list name cannot exceed 100 characters", nameof(name));

        Name = name;
        Description = description;
        SetUpdatedAt();

        AddDomainEvent(new TodoListUpdatedEvent(Id, name));
    }

    public void Archive()
    {
        IsArchived = true;
        SetUpdatedAt();

        AddDomainEvent(new TodoListArchivedEvent(Id, Name));
    }

    public void Restore()
    {
        IsArchived = false;
        SetUpdatedAt();

        AddDomainEvent(new TodoListRestoredEvent(Id, Name));
    }

    public void AddTodoItem(TodoItem todoItem)
    {
        if (IsArchived)
            throw new InvalidOperationException("Cannot add items to an archived list");

        _todoItems.Add(todoItem);
        SetUpdatedAt();
    }

    public void RemoveTodoItem(TodoItem todoItem)
    {
        _todoItems.Remove(todoItem);
        SetUpdatedAt();
    }
}