using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public sealed class TodoListCreatedEvent : IDomainEvent
{
    public int TodoListId { get; }
    public string Name { get; }
    public DateTime OccurredOn { get; }

    public TodoListCreatedEvent(int todoListId, string name)
    {
        TodoListId = todoListId;
        Name = name;
        OccurredOn = DateTime.UtcNow;
    }
}