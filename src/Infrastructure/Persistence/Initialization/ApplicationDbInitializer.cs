using Backend.Infrastructure.Persistence.Context;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Persistence.Initialization;

internal class ApplicationDbInitializer(
    ApplicationDbContext dbContext,
    ITenantInfo currentTenant,
    ApplicationDbSeeder dbSeeder,
    ILogger<ApplicationDbInitializer> logger)
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (dbContext.Database.GetMigrations().Any())
        {
            if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                logger.LogInformation("Applying Migrations for '{tenantId}' tenant.", currentTenant.Id);
                await dbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                logger.LogInformation("Connection to {tenantId}'s Database Succeeded.", currentTenant.Id);

                await dbSeeder.SeedDatabaseAsync(dbContext, cancellationToken);
            }
        }
    }
}
