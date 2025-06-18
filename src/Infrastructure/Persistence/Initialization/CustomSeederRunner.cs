using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Persistence.Initialization;

internal class CustomSeederRunner(IServiceProvider serviceProvider)
{
    private readonly ICustomSeeder[] _seeders = serviceProvider
        .GetServices<ICustomSeeder>()
        .ToArray();

    public async Task RunSeedersAsync(CancellationToken cancellationToken)
    {
        foreach (var seeder in _seeders)
        {
            await seeder.InitializeAsync(cancellationToken);
        }
    }
}
