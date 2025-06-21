using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public sealed class TodoItemCompletedEvent : IDomainEvent
{
    public int TodoItemId { get; }
    public string Title { get; }
    public DateTime OccurredOn { get; }

    public TodoItemCompletedEvent(int todoItemId, string title)
    {
        TodoItemId = todoItemId;
        Title = title;
        OccurredOn = DateTime.UtcNow;
    }
}