using CleanArchitecture.Application.Commands;
using CleanArchitecture.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Command Services (Write operations)
        services.AddScoped<ITodoItemCommandService, TodoItemCommandService>();
        // services.AddScoped<ITodoListCommandService, TodoListCommandService>(); // Add later

        // Query Services (Read operations)  
        services.AddScoped<ITodoItemQueryService, TodoItemQueryService>();
        // services.AddScoped<ITodoListQueryService, TodoListQueryService>(); // Add later

        // TODO: Add these when implementing advanced features:
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); // FluentValidation
        // services.AddAutoMapper(Assembly.GetExecutingAssembly()); // AutoMapper
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); // If switching to MediatR later

        return services;
    }
}