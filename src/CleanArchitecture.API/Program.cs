using CleanArchitecture.API.Middleware;
using CleanArchitecture.API.Services;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Persistence;
using Scalar.AspNetCore;

namespace CleanArchitecture.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container from each layer
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        // TODO: need to add middleware for exception handling, there is an integration test not working atm.

        // Configure the HTTP request pipeline
        ConfigurePipeline(app);

        app.Run();
    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // ===== LAYER REGISTRATIONS =====
        // Order matters for dependency resolution

        // Application Layer (Use Cases) - registers command/query services
        services.AddApplicationServices();

        // Infrastructure Layer (External Services) - implements Application interfaces
        services.AddInfrastructureServices(configuration);

        // Persistence Layer (Data Access) - implements Domain repositories and Application DbContext
        services.AddPersistenceServices(configuration);

        // ===== API LAYER SERVICES =====
        // HTTP Context for web-specific services
        services.AddHttpContextAccessor();

        // Web-specific services (presentation layer concerns)
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Controllers
        services.AddControllers();

        // API Documentation
        services.AddOpenApi();

        // TODO: Add these when needed:
        // Authentication & Authorization
        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options => { ... });
        // services.AddAuthorization(options => { ... });

        // CORS
        // services.AddCors(options => { ... });

        // API Versioning
        // services.AddApiVersioning();

        // Response Compression
        // services.AddResponseCompression();

        // Health Checks
        // services.AddHealthChecks();
    }

    public static void ConfigurePipeline(WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Clean Architecture TODO API";
                options.Theme = ScalarTheme.Purple;
            });
        }

        // TODO: Add production middleware:
        // if (app.Environment.IsProduction())
        // {
        //     app.UseHsts();
        // }

        // Standard middleware pipeline (order matters!)
        app.UseHttpsRedirection();

        // TODO: Add these when implementing:
        // app.UseCors();
        // app.UseAuthentication();
        // app.UseAuthorization();
        // app.UseResponseCompression();

        // Custom middleware (add when needed)
        // app.UseMiddleware<ExceptionHandlingMiddleware>();
        // app.UseMiddleware<LoggingMiddleware>();

        // Routing
        app.MapControllers();

        // TODO: Health checks endpoint
        // app.MapHealthChecks("/health");
    }
}