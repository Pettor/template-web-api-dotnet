using Backend.Infrastructure.Persistence.Context;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Persistence.Initialization;

internal class ApplicationDbInitializer(
    ApplicationDbContext dbContext,
    IMultiTenantContextAccessor accountContext,
    ApplicationDbSeeder dbSeeder,
    ILogger<ApplicationDbInitializer> logger
)
{
    private ITenantInfo CurrentTenant => accountContext.MultiTenantContext.TenantInfo!;

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!dbContext.Database.GetMigrations().Any())
        {
            return;
        }

        if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            logger.LogInformation("Applying Migrations for '{tenantId}' tenant.", CurrentTenant.Id);
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        if (await dbContext.Database.CanConnectAsync(cancellationToken))
        {
            logger.LogInformation(
                "Connection to {tenantId}'s Database Succeeded.",
                CurrentTenant.Id
            );

            await dbSeeder.SeedDatabaseAsync(dbContext, cancellationToken);
        }
    }
}
