using Hangfire;
using Hangfire.SqlServer;
using Hahn.CurrencyRates.Application;
using Hahn.CurrencyRates.Infrastructure;
using Hahn.CurrencyRates.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Add services to the container
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJobs();

// Configure Hangfire
builder.Services.AddHangfire((sp, config) => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), 
        new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddHangfireServer();

var host = builder.Build();

// Configure Hangfire jobs
var recurringJobManager = host.Services.GetRequiredService<IRecurringJobManager>();
Hahn.CurrencyRates.Jobs.DependencyInjection.ConfigureJobs(recurringJobManager);

await host.RunAsync();
