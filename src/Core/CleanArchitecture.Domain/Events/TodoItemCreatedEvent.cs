using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Events;

public sealed class TodoItemCreatedEvent : IDomainEvent
{
    public int TodoItemId { get; }
    public string Title { get; }
    public Priority Priority { get; }
    public DateTime OccurredOn { get; }

    public TodoItemCreatedEvent(int todoItemId, string title, Priority priority)
    {
        TodoItemId = todoItemId;
        Title = title;
        Priority = priority;
        OccurredOn = DateTime.UtcNow;
    }
}