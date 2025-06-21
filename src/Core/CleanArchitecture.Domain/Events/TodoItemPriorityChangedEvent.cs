using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Events;

public sealed class TodoItemPriorityChangedEvent : IDomainEvent
{
    public int TodoItemId { get; }
    public string Title { get; }
    public Priority NewPriority { get; }
    public DateTime OccurredOn { get; }

    public TodoItemPriorityChangedEvent(int todoItemId, string title, Priority newPriority)
    {
        TodoItemId = todoItemId;
        Title = title;
        NewPriority = newPriority;
        OccurredOn = DateTime.UtcNow;
    }
}