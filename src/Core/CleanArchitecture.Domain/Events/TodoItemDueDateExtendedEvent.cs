using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public sealed class TodoItemDueDateExtendedEvent : IDomainEvent
{
    public int TodoItemId { get; }
    public string Title { get; }
    public DateTime NewDueDate { get; }
    public DateTime OccurredOn { get; }

    public TodoItemDueDateExtendedEvent(int todoItemId, string title, DateTime newDueDate)
    {
        TodoItemId = todoItemId;
        Title = title;
        NewDueDate = newDueDate;
        OccurredOn = DateTime.UtcNow;
    }
}