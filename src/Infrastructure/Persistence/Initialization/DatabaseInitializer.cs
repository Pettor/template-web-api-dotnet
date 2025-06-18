using Backend.Infrastructure.Multitenancy;
using Backend.Shared.Multitenancy;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TenantInfo = Backend.Infrastructure.Multitenancy.TenantInfo;

namespace Backend.Infrastructure.Persistence.Initialization;

internal class DatabaseInitializer(
    TenantDbContext tenantDbContext,
    IOptions<DatabaseSettings> dbSettings,
    IServiceProvider serviceProvider,
    ILogger<DatabaseInitializer> logger
) : IDatabaseInitializer
{
    private readonly DatabaseSettings _dbSettings = dbSettings.Value;

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        await InitializeTenantDbAsync(cancellationToken);

        foreach (var tenant in await tenantDbContext.TenantInfo.ToListAsync(cancellationToken))
        {
            await InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
        }
    }

    public async Task InitializeApplicationDbForTenantAsync(
        TenantInfo tenant,
        CancellationToken cancellationToken
    )
    {
        // First create a new scope
        using var scope = serviceProvider.CreateScope();

        // Then set current tenant so the right connectionstring is used
        serviceProvider.GetRequiredService<IMultiTenantContextAccessor>().MultiTenantContext =
            new MultiTenantContext<TenantInfo> { TenantInfo = tenant };

        // Then run the initialization in the new scope
        await scope
            .ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
            .InitializeAsync(cancellationToken);
    }

    private async Task InitializeTenantDbAsync(CancellationToken cancellationToken)
    {
        var pendingMigrations = await tenantDbContext.Database.GetPendingMigrationsAsync(
            cancellationToken
        );
        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying Root Migrations.");
            await tenantDbContext.Database.MigrateAsync(cancellationToken);
        }

        await SeedRootTenantAsync(cancellationToken);
    }

    private async Task SeedRootTenantAsync(CancellationToken cancellationToken)
    {
        if (
            await tenantDbContext.TenantInfo.FindAsync(
                new object?[] { MultitenancyConstants.Root.Id },
                cancellationToken: cancellationToken
            )
            is null
        )
        {
            var rootTenant = new TenantInfo(
                MultitenancyConstants.Root.Id,
                MultitenancyConstants.Root.Name,
                _dbSettings.ConnectionString,
                MultitenancyConstants.Root.EmailAddress
            );

            rootTenant.SetValidity(DateTime.UtcNow.AddYears(1));

            tenantDbContext.TenantInfo.Add(rootTenant);

            await tenantDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
