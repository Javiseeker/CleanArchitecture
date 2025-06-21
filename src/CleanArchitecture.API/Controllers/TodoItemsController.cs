using CleanArchitecture.API.DTOs;
using CleanArchitecture.Application.Commands;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly ITodoItemCommandService _commandService;
    private readonly ITodoItemQueryService _queryService;

    public TodoItemsController(ITodoItemCommandService commandService, ITodoItemQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAll()
    {
        var todoItems = await _queryService.GetAllTodoItemsAsync();
        var dtos = todoItems.Select(item => new TodoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsCompleted = item.IsCompleted,
            DueDate = item.DueDate,
            Priority = item.Priority,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        });

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetById(int id)
    {
        var todoItem = await _queryService.GetTodoItemByIdAsync(id);
        if (todoItem == null)
            return NotFound();

        var dto = new TodoItemDto
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted,
            DueDate = todoItem.DueDate,
            Priority = todoItem.Priority,
            CreatedAt = todoItem.CreatedAt,
            UpdatedAt = todoItem.UpdatedAt
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemRequest request)
    {
        var todoItem = await _commandService.CreateTodoItemAsync(
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate);

        var dto = new TodoItemDto
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            IsCompleted = todoItem.IsCompleted,
            DueDate = todoItem.DueDate,
            Priority = todoItem.Priority,
            CreatedAt = todoItem.CreatedAt,
            UpdatedAt = todoItem.UpdatedAt
        };

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }
}