using WebApiTemplate.Infrastructure.Multitenancy;

namespace WebApiTemplate.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(FshTenantInfo tenant, CancellationToken cancellationToken);
}