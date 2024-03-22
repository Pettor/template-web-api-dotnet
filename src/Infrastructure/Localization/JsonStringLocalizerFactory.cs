using Backend.Application.Common.Caching;
using Microsoft.Extensions.Localization;

namespace Backend.Infrastructure.Localization;

public class JsonStringLocalizerFactory(ICacheService cache) : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource) =>
        new JsonStringLocalizer(cache);

    public IStringLocalizer Create(string baseName, string location) =>
        new JsonStringLocalizer(cache);
}
