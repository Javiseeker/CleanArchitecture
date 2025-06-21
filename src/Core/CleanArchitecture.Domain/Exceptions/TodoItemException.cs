namespace CleanArchitecture.Domain.Exceptions;

public class TodoItemException : DomainException
{
    public TodoItemException(string message) : base(message)
    {
    }

    public TodoItemException(string message, Exception innerException) : base(message, innerException)
    {
    }
}