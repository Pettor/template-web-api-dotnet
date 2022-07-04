using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using WebApiTemplate.Infrastructure.Common.Services;

namespace Infrastructure.Test.Caching;

public class DistributedCacheService : CacheService<WebApiTemplate.Infrastructure.Caching.DistributedCacheService>
{
    protected override WebApiTemplate.Infrastructure.Caching.DistributedCacheService CreateCacheService() =>
        new(
            new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())),
            new NewtonSoftService(),
            NullLogger<WebApiTemplate.Infrastructure.Caching.DistributedCacheService>.Instance);
}