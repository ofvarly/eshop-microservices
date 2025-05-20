using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add services to the container.

builder.Services.AddCarter(); // Add Carter for API routing

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly); // Add FluentValidation for request validation

builder.Services.AddMarten(opts => // Add Marten for data access
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!); // Set the connection string
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions(); // Use lightweight sessions for better performance

builder.Services.AddScoped<IBasketRepository, BasketRepository>(); // Add the repository for data access
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>(); // Decorate the repository with caching

builder.Services.AddStackExchangeRedisCache(options => // Add Redis cache
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = builder.Configuration["Redis:InstanceName"]!;
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // Add custom exception handler

builder.Services.AddHealthChecks() // Add health checks
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!) // Add PostgreSQL health check
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!); // Add Redis health check
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter(); // Map Carter routes
app.UseExceptionHandler(options => { }); // Use custom exception handler

app.UseHealthChecks("/health", // Add health check endpoint
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse // Use UI response writer to format the response
    });

app.Run();
