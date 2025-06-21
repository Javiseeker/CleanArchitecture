// tests/CleanArchitecture.Unit/Domain/Entities/TodoItemTests.cs
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Unit.Domain.Entities;

public class TodoItemTests
{
    [Fact]
    public void Create_WithValidTitle_ShouldCreateTodoItem()
    {
        // Arrange
        var title = "Test Todo Item";
        var description = "Test Description";
        var priority = Priority.High;
        var dueDate = DateTime.Today.AddDays(7);

        // Act
        var todoItem = TodoItem.Create(title, description, priority, dueDate);

        // Assert
        todoItem.Title.Should().Be(title);
        todoItem.Description.Should().Be(description);
        todoItem.Priority.Should().Be(priority);
        todoItem.DueDate.Should().Be(dueDate);
        todoItem.IsCompleted.Should().BeFalse();
        todoItem.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        todoItem.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Create_WithDefaultValues_ShouldCreateTodoItemWithMediumPriority()
    {
        // Arrange
        var title = "Simple Todo";

        // Act
        var todoItem = TodoItem.Create(title);

        // Assert
        todoItem.Title.Should().Be(title);
        todoItem.Description.Should().BeNull();
        todoItem.Priority.Should().Be(Priority.Medium);
        todoItem.DueDate.Should().BeNull();
        todoItem.IsCompleted.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidTitle_ShouldThrowDomainException(string invalidTitle)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => TodoItem.Create(invalidTitle));
        exception.Message.Should().Be("Todo item must have a title");
    }

    [Fact]
    public void Create_WithPastDueDate_ShouldThrowDomainException()
    {
        // Arrange
        var title = "Test Todo";
        var pastDate = DateTime.Today.AddDays(-1);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => TodoItem.Create(title, null, Priority.Medium, pastDate));
        exception.Message.Should().Be("Due date cannot be in the past");
    }

    [Fact]
    public void Create_WithCriticalPriorityAndDistantDueDate_ShouldThrowDomainException()
    {
        // Arrange
        var title = "Critical Task";
        var distantDate = DateTime.Today.AddDays(35); // More than 30 days

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            TodoItem.Create(title, null, Priority.Critical, distantDate));
        exception.Message.Should().Be("Critical priority items cannot have due dates more than 30 days out");
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateTodoItem()
    {
        // Arrange
        var todoItem = TodoItem.Create("Original Title");
        var newTitle = "Updated Title";
        var newDescription = "Updated Description";
        var newPriority = Priority.High;
        var newDueDate = DateTime.Today.AddDays(5);

        // Act
        todoItem.Update(newTitle, newDescription, newPriority, newDueDate);

        // Assert
        todoItem.Title.Should().Be(newTitle);
        todoItem.Description.Should().Be(newDescription);
        todoItem.Priority.Should().Be(newPriority);
        todoItem.DueDate.Should().Be(newDueDate);
        todoItem.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Update_WithInvalidTitle_ShouldThrowDomainException()
    {
        // Arrange
        var todoItem = TodoItem.Create("Original Title");

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() =>
            todoItem.Update("", "Description", Priority.Medium, null));
        exception.Message.Should().Be("Todo item must have a title");
    }

    [Fact]
    public void Complete_WhenNotCompleted_ShouldMarkAsCompleted()
    {
        // Arrange
        var todoItem = TodoItem.Create("Test Todo");

        // Act
        todoItem.Complete();

        // Assert
        todoItem.IsCompleted.Should().BeTrue();
        todoItem.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Complete_WhenAlreadyCompleted_ShouldThrowDomainException()
    {
        // Arrange
        var todoItem = TodoItem.Create("Test Todo");
        todoItem.Complete();

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => todoItem.Complete());
        exception.Message.Should().Be("Todo item is already completed");
    }

    [Fact]
    public void Reopen_WhenCompleted_ShouldMarkAsNotCompleted()
    {
        // Arrange
        var todoItem = TodoItem.Create("Test Todo");
        todoItem.Complete();

        // Act
        todoItem.Reopen();

        // Assert
        todoItem.IsCompleted.Should().BeFalse();
        todoItem.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Reopen_WhenNotCompleted_ShouldThrowDomainException()
    {
        // Arrange
        var todoItem = TodoItem.Create("Test Todo");

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => todoItem.Reopen());
        exception.Message.Should().Be("Todo item is not completed");
    }

    [Fact]
    public void DomainEvents_WhenCreated_ShouldBeEmpty()
    {
        // Arrange & Act
        var todoItem = TodoItem.Create("Test Todo");

        // Assert
        todoItem.DomainEvents.Should().BeEmpty();
    }
}