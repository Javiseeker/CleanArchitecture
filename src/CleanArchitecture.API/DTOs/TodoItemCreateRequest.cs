using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.API.DTOs;

public class CreateTodoItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }
}