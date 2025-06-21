using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public sealed class TodoListUpdatedEvent : IDomainEvent
{
    public int TodoListId { get; }
    public string Name { get; }
    public DateTime OccurredOn { get; }

    public TodoListUpdatedEvent(int todoListId, string name)
    {
        TodoListId = todoListId;
        Name = name;
        OccurredOn = DateTime.UtcNow;
    }
}