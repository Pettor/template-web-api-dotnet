using Backend.Infrastructure.Multitenancy;

namespace Backend.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(FshTenantInfo tenant, CancellationToken cancellationToken);
}