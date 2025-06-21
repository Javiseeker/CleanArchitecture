using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Persistence.Context;
using CleanArchitecture.Persistence.Repositories;
using CleanArchitecture.Persistence.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();

        services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        services.AddScoped<ITodoListRepository, TodoListRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // TODO: Database Health Checks
        // services.AddHealthChecks()
        //     .AddDbContextCheck<ApplicationDbContext>();

        // TODO: Switch to real database later:
        // services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlServer(
        //         configuration.GetConnectionString("DefaultConnection"),
        //         b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        return services;
    }
}