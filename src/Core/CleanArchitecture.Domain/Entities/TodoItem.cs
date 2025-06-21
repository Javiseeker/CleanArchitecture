using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Events;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Entities;

public class TodoItem : BaseEntity<int>
{
    public string Title { get; private set; } = string.Empty;
    public Description Description { get; private set; } = Description.Create(null);
    public Priority Priority { get; private set; }
    public TodoItemStatus Status { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public int TodoListId { get; private set; }

    // Navigation properties
    public TodoList TodoList { get; private set; } = null!;

    // Calculated properties
    public bool IsCompleted => Status == TodoItemStatus.Completed;
    public bool IsCancelled => Status == TodoItemStatus.Cancelled;
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && !IsCompleted;
    public bool IsDueSoon => DueDate.HasValue && DueDate.Value <= DateTime.UtcNow.AddDays(1) && !IsCompleted;
    public int DaysUntilDue => DueDate?.Subtract(DateTime.UtcNow).Days ?? int.MaxValue;

    private TodoItem() { } // For EF Core

    private TodoItem(string title, string? description, Priority priority, DateTime? dueDate, int todoListId)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = Description.Create(description);
        Priority = priority;
        Status = TodoItemStatus.Pending;
        DueDate = dueDate;
        TodoListId = todoListId;

        AddDomainEvent(new TodoItemCreatedEvent(Id, title, priority));
    }

    public static TodoItem Create(string title, string? description, Priority priority, DateTime? dueDate, int todoListId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Todo item title cannot be empty", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Todo item title cannot exceed 200 characters", nameof(title));

        if (dueDate.HasValue && dueDate.Value < DateTime.UtcNow)
            throw new ArgumentException("Due date cannot be in the past", nameof(dueDate));

        return new TodoItem(title, description, priority, dueDate, todoListId);
    }

    public void Update(string title, string? description, Priority priority, DateTime? dueDate)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot update a completed todo item");

        if (IsCancelled)
            throw new InvalidOperationException("Cannot update a cancelled todo item");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Todo item title cannot be empty", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Todo item title cannot exceed 200 characters", nameof(title));

        Title = title;
        Description = Description.Create(description);
        Priority = priority;
        DueDate = dueDate;
        SetUpdatedAt();

        AddDomainEvent(new TodoItemUpdatedEvent(Id, title, priority));
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Todo item is already completed");

        if (IsCancelled)
            throw new InvalidOperationException("Cannot complete a cancelled todo item");

        Status = TodoItemStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        SetUpdatedAt();

        AddDomainEvent(new TodoItemCompletedEvent(Id, Title));
    }

    public void Reopen()
    {
        if (!IsCompleted)
            throw new InvalidOperationException("Todo item is not completed");

        Status = TodoItemStatus.Pending;
        CompletedAt = null;
        SetUpdatedAt();

        AddDomainEvent(new TodoItemReopenedEvent(Id, Title));
    }

    public void Cancel()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot cancel a completed todo item");

        if (IsCancelled)
            throw new InvalidOperationException("Todo item is already cancelled");

        Status = TodoItemStatus.Cancelled;
        SetUpdatedAt();

        AddDomainEvent(new TodoItemCancelledEvent(Id, Title));
    }

    public void SetInProgress()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot set completed item as in progress");

        if (IsCancelled)
            throw new InvalidOperationException("Cannot set cancelled item as in progress");

        Status = TodoItemStatus.InProgress;
        SetUpdatedAt();

        AddDomainEvent(new TodoItemStatusChangedEvent(Id, Title, Status));
    }

    public void ChangePriority(Priority newPriority)
    {
        if (Priority == newPriority)
            return;

        Priority = newPriority;
        SetUpdatedAt();

        AddDomainEvent(new TodoItemPriorityChangedEvent(Id, Title, newPriority));
    }

    public void ExtendDueDate(DateTime newDueDate)
    {
        if (newDueDate < DateTime.UtcNow)
            throw new ArgumentException("New due date cannot be in the past", nameof(newDueDate));

        if (DueDate.HasValue && newDueDate <= DueDate.Value)
            throw new ArgumentException("New due date must be later than current due date", nameof(newDueDate));

        DueDate = newDueDate;
        SetUpdatedAt();

        AddDomainEvent(new TodoItemDueDateExtendedEvent(Id, Title, newDueDate));
    }
}