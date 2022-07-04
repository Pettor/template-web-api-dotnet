using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching;

public class LocalCacheService : CacheService<WebApiTemplate.Infrastructure.Caching.LocalCacheService>
{
    protected override WebApiTemplate.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
        new(
            new MemoryCache(new MemoryCacheOptions()),
            NullLogger<WebApiTemplate.Infrastructure.Caching.LocalCacheService>.Instance);
}