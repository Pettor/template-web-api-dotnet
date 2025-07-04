﻿using Backend.Application.Common.Persistence;
using Backend.Domain.Common.Contracts;
using Backend.Infrastructure.Common;
using Backend.Infrastructure.Persistence.ConnectionString;
using Backend.Infrastructure.Persistence.Context;
using Backend.Infrastructure.Persistence.Initialization;
using Backend.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Backend.Infrastructure.Persistence;

internal static class Startup
{
    private static readonly ILogger Logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        // TODO: there must be a cleaner way to do IOptions validation...
        var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        var rootConnectionString = databaseSettings!.ConnectionString;
        if (string.IsNullOrEmpty(rootConnectionString))
        {
            throw new InvalidOperationException("DB ConnectionString is not configured.");
        }

        var dbProvider = databaseSettings.DbProvider;
        if (string.IsNullOrEmpty(dbProvider))
        {
            throw new InvalidOperationException("DB Provider is not configured.");
        }

        Logger.Information($"Current DB Provider : {dbProvider}");

        return services
            .Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))
            .AddDbContext<ApplicationDbContext>(m =>
                m.UseDatabase(dbProvider, rootConnectionString)
                    .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
            )
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<ApplicationDbInitializer>()
            .AddTransient<ApplicationDbSeeder>()
            .AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient)
            .AddTransient<CustomSeederRunner>()
            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
            .AddTransient<IConnectionStringValidator, ConnectionStringValidator>()
            .AddRepositories();
    }

    internal static DbContextOptionsBuilder UseDatabase(
        this DbContextOptionsBuilder builder,
        string dbProvider,
        string connectionString
    )
    {
        switch (dbProvider.ToUpperInvariant())
        {
            case DbProviderKeys.Npgsql:
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                return builder.UseNpgsql(
                    connectionString,
                    e => e.MigrationsAssembly("Migrators.PostgreSQL")
                );

            default:
                throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
        }
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Add Repositories
        services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

        foreach (
            var aggregateRootType in typeof(IAggregateRoot)
                .Assembly.GetExportedTypes()
                .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                .ToList()
        )
        {
            // Add ReadRepositories.
            services.AddScoped(
                typeof(IReadRepository<>).MakeGenericType(aggregateRootType),
                sp =>
                    sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType))
            );

            // Decorate the repositories with EventAddingRepositoryDecorators and expose them as IRepositoryWithEvents.
            services.AddScoped(
                typeof(IRepositoryWithEvents<>).MakeGenericType(aggregateRootType),
                sp =>
                    Activator.CreateInstance(
                        typeof(EventAddingRepositoryDecorator<>).MakeGenericType(aggregateRootType),
                        sp.GetRequiredService(
                            typeof(IRepository<>).MakeGenericType(aggregateRootType)
                        )
                    )
                    ?? throw new InvalidOperationException(
                        $"Couldn't create EventAddingRepositoryDecorator for aggregateRootType {aggregateRootType.Name}"
                    )
            );
        }

        return services;
    }
}
