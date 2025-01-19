using Hahn.CurrencyRates.Application;
using Hahn.CurrencyRates.Infrastructure;
using Hahn.CurrencyRates.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS for Vue.js frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Default Vite dev server port
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add application services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, includeQueryContext: true);

var app = builder.Build();

// Apply migrations for both command and query databases
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Applying command database migrations...");
        var commandDb = services.GetRequiredService<CurrencyRatesDbContext>();
        await commandDb.Database.MigrateAsync();

        logger.LogInformation("Applying query database migrations...");
        var queryDb = services.GetRequiredService<QueryDbContext>();
        await queryDb.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the databases");
        throw; // Re-throw to prevent application startup if migrations fail
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("VueApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
