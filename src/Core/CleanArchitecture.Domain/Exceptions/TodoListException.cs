namespace CleanArchitecture.Domain.Exceptions;

public class TodoListException : DomainException
{
    public TodoListException(string message) : base(message)
    {
    }

    public TodoListException(string message, Exception innerException) : base(message, innerException)
    {
    }
}