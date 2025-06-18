using Backend.Infrastructure.Multitenancy;

namespace Backend.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(
        TenantInfo tenant,
        CancellationToken cancellationToken
    );
}
