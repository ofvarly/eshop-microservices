using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add services to the container.
builder.Services.AddCarter(); // Add Carter for API routing
builder.Services.AddMediatR(config => // Add MediatR for CQRS
{
    config.RegisterServicesFromAssembly(assembly); // Register all handlers in the assembly
    config.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Add validation behavior
    config.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Add logging behavior
});

builder.Services.AddValidatorsFromAssembly(assembly); // Add FluentValidation for request validation

builder.Services.AddMarten(opts => // Add Marten for data access
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!); // Set the connection string
}).UseLightweightSessions(); // Use lightweight sessions for better performance

if (builder.Environment.IsDevelopment()) // Add development services
    builder.Services.InitializeMartenWith<CatalogInitialData>(); // Initialize Marten with initial data

builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // Add custom exception handler

builder.Services.AddHealthChecks() // Add health checks
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!); // Add PostgreSQL health check

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter(); // Map Carter routes

app.UseExceptionHandler(options => {}); // Use custom exception handler

app.UseHealthChecks("/health", // Add health check endpoint
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse // Use UI response writer to format the response
    });

app.Run();
