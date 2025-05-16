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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter(); // Map Carter routes

app.Run();
