using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Persistence;
using Scalar.AspNetCore;

namespace CleanArchitecture;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container from each layer
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline
        ConfigurePipeline(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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

    private static void ConfigurePipeline(WebApplication app)
    {
        // Development-specific middleware
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