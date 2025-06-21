using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CleanArchitecture.API;
using CleanArchitecture.API.DTOs;
using CleanArchitecture.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CleanArchitecture.Integration.Controllers;

public class TodoItemsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public TodoItemsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        // Configure JSON options to match the API's configuration
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithTodoItems()
    {
        // Act
        var response = await _client.GetAsync("/api/todoitems");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var todoItems = JsonSerializer.Deserialize<List<TodoItemResponse>>(content, _jsonOptions);

        todoItems.Should().NotBeNull();
        todoItems.Should().HaveCountGreaterThan(0); // We have seeded data

        // Verify the structure of returned items
        var firstItem = todoItems!.First();
        firstItem.Id.Should().BeGreaterThan(0);
        firstItem.Title.Should().NotBeNullOrEmpty();
        firstItem.Priority.Should().BeDefined();
        firstItem.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromDays(1));
    }

    [Fact]
    public async Task GetById_WithExistingId_ShouldReturnOkWithTodoItem()
    {
        // Arrange - First get all items to find a valid ID
        var getAllResponse = await _client.GetAsync("/api/todoitems");
        var allItems = JsonSerializer.Deserialize<List<TodoItemResponse>>(
            await getAllResponse.Content.ReadAsStringAsync(), _jsonOptions);
        var existingId = allItems!.First().Id;

        // Act
        var response = await _client.GetAsync($"/api/todoitems/{existingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var todoItem = JsonSerializer.Deserialize<TodoItemResponse>(content, _jsonOptions);

        todoItem.Should().NotBeNull();
        todoItem!.Id.Should().Be(existingId);
        todoItem.Title.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetById_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = 99999;

        // Act
        var response = await _client.GetAsync($"/api/todoitems/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithMinimalValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var request = new TodoItemCreateRequest
        {
            Title = "Minimal Todo"
            // Only title provided, others should use defaults
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/todoitems", request, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadAsStringAsync();
        var createdItem = JsonSerializer.Deserialize<TodoItemResponse>(content, _jsonOptions);

        createdItem.Should().NotBeNull();
        createdItem!.Title.Should().Be(request.Title);
        createdItem.Description.Should().BeNull();
        createdItem.Priority.Should().Be(Priority.Medium); // Default value
        createdItem.DueDate.Should().BeNull();
    }

    [Fact]
    public async Task Create_WithEmptyTitle_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new TodoItemCreateRequest
        {
            Title = "", // Invalid empty title
            Priority = Priority.Medium
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/todoitems", request, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ThenGet_ShouldReturnSameTodoItem()
    {
        // Arrange
        var request = new TodoItemCreateRequest
        {
            Title = "Round Trip Test Todo",
            Description = "Testing create then get",
            Priority = Priority.Low,
            DueDate = DateTime.Today.AddDays(3)
        };

        // Act 1: Create
        var createResponse = await _client.PostAsJsonAsync("/api/todoitems", request, _jsonOptions);
        var createdItem = JsonSerializer.Deserialize<TodoItemResponse>(
            await createResponse.Content.ReadAsStringAsync(), _jsonOptions);

        // Act 2: Get the created item
        var getResponse = await _client.GetAsync($"/api/todoitems/{createdItem!.Id}");
        var retrievedItem = JsonSerializer.Deserialize<TodoItemResponse>(
            await getResponse.Content.ReadAsStringAsync(), _jsonOptions);

        // Assert
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        retrievedItem.Should().BeEquivalentTo(createdItem);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _client?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}