using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: External Services (implement these later)
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        // How can I implement the InMemoryCacheService here?

        // TODO: Caching (when adding Redis or Memory cache)
        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = configuration.GetConnectionString("Redis");
        // });


        // TODO: HTTP Clients (when integrating with external APIs)
        // services.AddHttpClient<IExternalApiClient, ExternalApiClient>();

        // TODO: Background Services (when adding hosted services)
        // services.AddHostedService<BackgroundTaskService>();

        return services;
    }
}