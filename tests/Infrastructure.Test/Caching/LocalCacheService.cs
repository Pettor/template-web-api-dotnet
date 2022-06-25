using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.Test.Caching;

public class LocalCacheService : CacheService<MyHero.Infrastructure.Caching.LocalCacheService>
{
    protected override MyHero.Infrastructure.Caching.LocalCacheService CreateCacheService() =>
        new(
            new MemoryCache(new MemoryCacheOptions()),
            NullLogger<MyHero.Infrastructure.Caching.LocalCacheService>.Instance);
}