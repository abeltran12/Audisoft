using Audisoft.Api.Middleware;
using Audisoft.Application.Extensions;
using Audisoft.Infrastructure.ApplicationContext;
using Audisoft.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MiPolitica", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Pagination");
    });
});

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Audisoft API",
            Version = "v1",
            Description = """
                <img src="/images/logos.png" height="300" />

                API para la gestión de estudiantes, profesores y notas del sistema Audisoft.
                """
        };

        return Task.CompletedTask;
    });
});


var app = builder.Build();

app.UseExceptionHandler();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        await DbSeeder.SeedAsync(context);
    }

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithFavicon("/images/fav.jpg")
            .WithTitle("Api Specification | Audisoft")
            .WithTheme(ScalarTheme.BluePlanet);
    });
}

app.UseCors("MiPolitica");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();