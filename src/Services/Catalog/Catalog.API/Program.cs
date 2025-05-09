using BuildingBlocks.Behaviors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
})
.UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

// Uygulama genelinde bir hata yakalama mekanizmasý tanýmlanýyor.
app.UseExceptionHandler(exceptionHandlerApp =>
{
    // Hata yakalandýðýnda çalýþtýrýlacak bir middleware tanýmlanýyor.
    exceptionHandlerApp.Run(async context =>
    {
        // Hata detaylarýný almak için `IExceptionHandlerFeature` kullanýlýyor.
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        // Eðer bir hata yoksa (örneðin, null ise), iþlem sonlandýrýlýyor.
        if (exception is null) return;

        // Hata detaylarýný içeren bir `ProblemDetails` nesnesi oluþturuluyor.
        var problemDetails = new ProblemDetails
        {
            // Hata mesajý, `ProblemDetails` nesnesinin baþlýðý olarak ayarlanýyor.
            Title = exception.Message,

            // HTTP durum kodu 500 (Internal Server Error) olarak ayarlanýyor.
            Status = StatusCodes.Status500InternalServerError,

            // Hatanýn ayrýntýlarý (örneðin, stack trace) `Detail` alanýna ekleniyor.
            Detail = exception.StackTrace
        };

        // Uygulamanýn `ILogger` servisi kullanýlarak hata loglanýyor.
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, exception.Message);

        // HTTP yanýtýnýn durum kodu 500 olarak ayarlanýyor.
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // Yanýtýn içeriði `application/problem+json` olarak ayarlanýyor.
        context.Response.ContentType = "application/problem+json";

        // Oluþturulan `ProblemDetails` nesnesi JSON formatýnda istemciye döndürülüyor.
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.Run();
