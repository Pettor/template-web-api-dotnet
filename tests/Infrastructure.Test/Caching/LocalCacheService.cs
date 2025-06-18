using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching;

public class LocalCacheService : CacheService<Backend.Infrastructure.Caching.LocalCacheService>
{
    protected override Backend.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
        new(
            new MemoryCache(new MemoryCacheOptions()),
            NullLogger<Backend.Infrastructure.Caching.LocalCacheService>.Instance
        );
}
