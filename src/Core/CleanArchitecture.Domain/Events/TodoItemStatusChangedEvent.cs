using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Events;

public sealed class TodoItemStatusChangedEvent : IDomainEvent
{
    public int TodoItemId { get; }
    public string Title { get; }
    public TodoItemStatus NewStatus { get; }
    public DateTime OccurredOn { get; }

    public TodoItemStatusChangedEvent(int todoItemId, string title, TodoItemStatus newStatus)
    {
        TodoItemId = todoItemId;
        Title = title;
        NewStatus = newStatus;
        OccurredOn = DateTime.UtcNow;
    }
}