using Backend.Application.Common.Caching;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;

namespace Backend.Infrastructure.Caching;

public class CacheKeyService(ITenantInfo currentTenant) : ICacheKeyService
{
    private readonly ITenantInfo? _currentTenant = currentTenant;

    public string GetCacheKey(string name, object id, bool includeTenantId = true)
    {
        var tenantId = includeTenantId
            ? _currentTenant?.Id
                ?? throw new InvalidOperationException(
                    "GetCacheKey: includeTenantId set to true and no ITenantInfo available."
                )
            : "GLOBAL";
        return $"{tenantId}-{name}-{id}";
    }
}
