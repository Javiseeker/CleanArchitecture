// src/Core/CleanArchitecture.Domain/Entities/TodoItem.cs

using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Exceptions;

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
        // ===== DOMAIN BUSINESS RULES =====
        // These are core business rules that should ALWAYS be enforced

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo item must have a title");

        // Business rule: Due date cannot be in the past
        if (dueDate.HasValue && dueDate.Value.Date < DateTime.Today)
            throw new DomainException("Due date cannot be in the past");

        // Business rule: Critical priority items cannot have due dates more than 30 days out
        if (priority == Priority.Critical && dueDate.HasValue && dueDate.Value > DateTime.Today.AddDays(30))
            throw new DomainException("Critical priority items cannot have due dates more than 30 days out");

        return new TodoItem(title, description, priority, dueDate);
    }

    public void Update(string title, string? description, Priority priority, DateTime? dueDate)
    {
        // Same business rules apply to updates
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo item must have a title");

        if (dueDate.HasValue && dueDate.Value.Date < DateTime.Today)
            throw new DomainException("Due date cannot be in the past");

        if (priority == Priority.Critical && dueDate.HasValue && dueDate.Value > DateTime.Today.AddDays(30))
            throw new DomainException("Critical priority items cannot have due dates more than 30 days out");

        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        SetUpdatedAt();
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new DomainException("Todo item is already completed");

        IsCompleted = true;
        SetUpdatedAt();
    }

    public void Reopen()
    {
        if (!IsCompleted)
            throw new DomainException("Todo item is not completed");

        IsCompleted = false;
        SetUpdatedAt();
    }
}