using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class TodoItem : BaseEntity<int>
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Priority Priority { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? DueDate { get; private set; }

    private TodoItem() { } // For EF Core

    private TodoItem(string title, string? description, Priority priority, DateTime? dueDate)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        Priority = priority;
        IsCompleted = false;
        DueDate = dueDate;
    }

    public static TodoItem Create(string title, string? description = null, Priority priority = Priority.Medium, DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Todo item title cannot be empty", nameof(title));

        return new TodoItem(title, description, priority, dueDate);
    }

    public void Update(string title, string? description, Priority priority, DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Todo item title cannot be empty", nameof(title));

        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        SetUpdatedAt();
    }

    public void Complete()
    {
        IsCompleted = true;
        SetUpdatedAt();
    }

    public void Reopen()
    {
        IsCompleted = false;
        SetUpdatedAt();
    }
}