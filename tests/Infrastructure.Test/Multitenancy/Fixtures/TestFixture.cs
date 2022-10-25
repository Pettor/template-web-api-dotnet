using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiTemplate.Application.Common.Persistence;
using WebApiTemplate.Infrastructure.Persistence.ConnectionString;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Infrastructure.Test.Multitenancy.Fixtures;

public abstract class TestFixture : TestBedFixture
{
    protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        => services
            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>();

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new TestAppSettings
        {
            Filename = "appsettings.json"
        };
    }

    protected override ValueTask DisposeAsyncCore()
        => new();
}