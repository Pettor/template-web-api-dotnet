using Backend.Application;
using Backend.Host;
using Backend.Host.Configurations;
using Backend.Host.Controllers;
using Backend.Infrastructure;
using Backend.Infrastructure.Common;
using FluentValidation.AspNetCore;
using Serilog;

[assembly: ApiConventionType(typeof(ApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations();
    builder.AddSerilog();

    builder.Services.AddControllers();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.MapEndpoints();
    await app.RunAsync();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    await Log.CloseAndFlushAsync();
}