using Movies.Api.Mapping;
using Movies.Application;
using Movies.Application.Database;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = builder.Configuration;

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddApplication();
        builder.Services.AddDatabase(config["Database:ConnectionString"]!);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseMiddleware<ValidationMappingMiddleware>();

        app.MapControllers();

        var dbInitializer = app.Services.GetService<DbInitializer>();
        await dbInitializer.InitializeAsync();

        app.Run();
    }
}