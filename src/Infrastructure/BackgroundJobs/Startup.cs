using System.Configuration;
using Backend.Infrastructure.Common;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Backend.Infrastructure.BackgroundJobs;

internal static class Startup
{
    private static readonly ILogger Logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddBackgroundJobs(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddHangfireServer(options =>
            config.GetSection("HangfireSettings:Server").Bind(options)
        );

        services.AddHangfireConsoleExtensions();

        var storageSettings = config
            .GetSection("HangfireSettings:Storage")
            .Get<HangfireStorageSettings>();

        if (string.IsNullOrEmpty(storageSettings!.StorageProvider))
            throw new ConfigurationErrorsException("Hangfire Storage Provider is not configured.");
        if (string.IsNullOrEmpty(storageSettings!.ConnectionString))
            throw new ConfigurationErrorsException(
                "Hangfire Storage Provider ConnectionString is not configured."
            );
        Logger.Information(
            "Hangfire: Current Storage Provider : {StorageProvider}",
            storageSettings!.StorageProvider
        );
        Logger.Information(
            "For more Hangfire storage, visit https://www.hangfire.io/extensions.html"
        );

        services.AddSingleton<JobActivator, DefaultJobActivator>();

        services.AddHangfire(
            (provider, hangfireConfig) =>
                hangfireConfig
                    .UseDatabase(
                        storageSettings.StorageProvider,
                        storageSettings.ConnectionString,
                        config
                    )
                    .UseFilter(new DefaultJobFilter(provider))
                    .UseFilter(new LogJobFilter())
                    .UseConsole()
        );

        return services;
    }

    private static IGlobalConfiguration UseDatabase(
        this IGlobalConfiguration hangfireConfig,
        string dbProvider,
        string connectionString,
        IConfiguration config
    ) =>
        dbProvider.ToUpperInvariant() switch
        {
            DbProviderKeys.Npgsql => hangfireConfig.UsePostgreSqlStorage(
                (options) =>
                {
                    options.UseNpgsqlConnection(connectionString);
                },
                config
                    .GetSection("HangfireSettings:Storage:Options")
                    .Get<PostgreSqlStorageOptions>()
            ),
            _ => throw new ConfigurationErrorsException(
                $"Hangfire Storage Provider {dbProvider} is not supported."
            ),
        };

    internal static IApplicationBuilder UseHangfireDashboard(
        this IApplicationBuilder app,
        IConfiguration config
    )
    {
        var dashboardOptions = config
            .GetSection("HangfireSettings:Dashboard")
            .Get<DashboardOptions>();

        dashboardOptions!.Authorization = new[]
        {
            new HangfireCustomBasicAuthenticationFilter
            {
                User = config.GetSection("HangfireSettings:Credentials:User").Value,
                Pass = config.GetSection("HangfireSettings:Credentials:Password").Value,
            },
        };

        return app.UseHangfireDashboard(config["HangfireSettings:Route"], dashboardOptions);
    }
}
